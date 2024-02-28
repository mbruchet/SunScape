using Atc.Rest.MinimalApi.Extensions;
using Atc.Rest.MinimalApi.Filters.Endpoints;
using Atc.Rest.MinimalApi.Middleware;
using Atc.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using SunScape.Api;
using SunScape.Components;
using SunScape.Data;
using SunScape.Identity;
using SunScape.Services;

namespace SunScape
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add APi
            builder.Services.ConfigureApiVersioning();

            builder.Services.AddEndpointDefinitions(typeof(IApiContractAssemblyMarker));

            builder.Services.ConfigureSwagger();


            //TODO Identity 3. Register the DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //TODO Identity 4. Add Cascading Authentication and Identity User Accessor and Redirect Manager
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();

            //TODO SSO 1. Add Google Authentication
            builder.Services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Google:client_id"];
                options.ClientSecret = builder.Configuration["Google:client_secret"];
            });

            builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

            ///TODO Identity 5. Add Identity Core with Default Token Provider
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

           // builder.Services.AddIdentityApiEndpoints<ApplicationUser>();

            //TODO Identity 7. Register Email Sender Service
            builder.Services.RegisterEmailService(builder.Configuration);

            // TODO Localization 2. Register a local service to list cultures 
            builder.Services.AddSingleton<CultureService>();

            /// TODO Localization 3. Add Localization middleware with folder path
            builder.Services.AddLocalization();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            // This enables proper enum as string in Swagger UI
            builder.Services.AddControllers().AddJsonOptions(o => JsonSerializerOptionsFactory.Create());
            builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o => JsonSerializerOptionsFactory.Create());

            builder.Services.AddSingleton(_ => new ValidationFilterOptions
            {
                SkipFirstLevelOnValidationKeys = true,
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Configurez le statut de réponse pour les demandes non autorisées
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            builder.Services.AddAntiforgery();

            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            app.UseEndpointDefinitions();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            // Use Swagger
            app.ConfigureSwaggerUI(builder.Environment.ApplicationName);

            // TODO Identity 9 : Call The migration of the identity database
            try
            {
                using var scope = app.Services.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
                context.Database.Migrate();
                
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                //TODO Identity 10 : Seed Admin account
                await SeedAdminAccount.SeedAdminUserAsync(userManager, configuration);
            }
            catch (Exception ex)
            {
                // Logique de gestion des exceptions
                Console.WriteLine(ex.ToString());
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // TODO Localization 4. Get culture service instance from DI
            var cultureService = app.Services.GetRequiredService<CultureService>();

            // TODO Localization 5. Get supported cultures
            string[] supportedCultures = cultureService.GetSupportedCultures().ToArray() ;

            // TODO Localization 6. Define supported cultures
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            // TODO Localization 7. Add on first position cookie detection 
            localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

            // TODO Localization 8. Use request localization
            app.UseRequestLocalization(localizationOptions);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 401)
                {
                    context.Response.Redirect("/AccessDenied");
                }
            });


            //Map Culture Set and use Minimal API Approach
            app.MapGet("/Culture/Set", (string culture, string redirectUri, HttpContext httpContext) =>
            {
                if (!string.IsNullOrWhiteSpace(culture))
                {
                    var requestCulture = new RequestCulture(culture, culture);
                    var cookieName = CookieRequestCultureProvider.DefaultCookieName;
                    var cookieValue = CookieRequestCultureProvider.MakeCookieValue(requestCulture);

                    httpContext.Response.Cookies.Append(cookieName, cookieValue,
                        new CookieOptions { Path = "/", Expires = DateTimeOffset.Now.AddDays(14),
                            HttpOnly = true, 
                            IsEssential = true,  SameSite = SameSiteMode.Lax });
                }

                return Results.LocalRedirect(redirectUri);
            });

            app.MapGet("/Account/Logout", async (HttpContext httpContext, SignInManager<ApplicationUser> signInManager) =>
            {
                if(httpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    await signInManager.SignOutAsync();
                }

                return Results.LocalRedirect("/");
            });

            // TODO Identity 8. Add additional endpoints required by the Identity /Account Razor components.
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapIdentityApi<ApplicationUser>();
            app.MapAdditionalIdentityEndpoints();

            app.UseAntiforgery();

            app.Run();
        }
    }
}
