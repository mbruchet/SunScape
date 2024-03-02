using Atc.Rest.MinimalApi.Extensions;
using Atc.Rest.MinimalApi.Filters.Endpoints;
using Atc.Rest.MinimalApi.Middleware;
using Atc.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SunScape.Api;
using SunScape.Components;
using SunScape.Data;
using SunScape.Identity;
using SunScape.Identity.Policies;
using SunScape.Services;

namespace SunScape;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var databaseProvider = builder.Configuration["DatabaseProvider"] ?? "SqlLite";

        try
        {
            /************************************************************************************************
             * 1. Add the required services to the container
             * 2. Configure the HTTP request pipeline
             * 3. Seed the database with the admin account
             * 4. Run the application
             ************************************************************************************************/

            //Add APi
            builder.Services.ConfigureApiVersioning();

            builder.Services.AddEndpointDefinitions(typeof(IApiContractAssemblyMarker));

            builder.Services.ConfigureSwagger();

            //TODO Identity 3. Register the DbContext
            var connectionString = builder.Configuration.GetConnectionString("IDDatabase")
                ?? "Data Source=SunScapeIdentityDb.db";

            if (builder.Environment.IsDevelopment() || builder.Environment.IsDocker())
            {
                Console.WriteLine($"Db Id ConnectionStrings:{connectionString}");
            }

            switch (databaseProvider.ToLower())
            {
                case "sqlserver":
                    {
                        builder.Services.AddDbContext<ApplicationSqlServerIdentityDbContext>(options =>
                                   {
                                       var optSqlBuilder = options.UseSqlServer(connectionString);
                                       if (builder.Environment.IsDevelopment() || builder.Environment.IsDocker())
                                       {
                                           optSqlBuilder.EnableSensitiveDataLogging().EnableDetailedErrors();
                                       }
                                   });

                        builder.Services.AddHealthChecks()
                            .AddDbContextCheck<ApplicationSqlServerIdentityDbContext>();
                        break;
                    }
                case "sqllite":
                    {
                        builder.Services.AddDbContext<ApplicationSqlLiteIdentityDbContext>(options =>
                           options.UseSqlite(connectionString));

                        builder.Services.AddHealthChecks()
                            .AddDbContextCheck<ApplicationSqlLiteIdentityDbContext>();
                        break;
                    }
                case "npgsql":
                    {
                        builder.Services.AddDbContext<ApplicationNpgsqlIdentityDbContext>(options =>
                            options.UseNpgsql(connectionString));
                        builder.Services.AddHealthChecks()
                            .AddDbContextCheck<ApplicationNpgsqlIdentityDbContext>();
                        break;
                    }
                case "inmemory":
                    builder.Services.AddDbContext<ApplicationSqlServerIdentityDbContext>(options =>
                                           options.UseInMemoryDatabase("SunScapeIdentityDb"));

                    builder.Services.AddHealthChecks()
                        .AddDbContextCheck<ApplicationSqlServerIdentityDbContext>();
                    break;
                default:
                    throw new InvalidOperationException("Database provider not supported.");
            }

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //TODO Identity 4. Add Cascading Authentication and Identity User Accessor and Redirect Manager
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();

            //TODO SSO 1. Add Google Authentication
            var googleClientId = builder.Configuration["Google:client_id"];

            if (!string.IsNullOrEmpty(googleClientId))
            {
                builder.Services.AddAuthentication().AddGoogle(options =>
                {
                    options.ClientId = googleClientId;
                    options.ClientSecret = builder.Configuration["Google:client_secret"] ?? string.Empty;
                });
            }

            builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

            IdentityBuilder identityBuilder;

            ///TODO Identity 5. Add Identity Core with Default Token Provider
            switch (databaseProvider.ToLower())
            {
                case "sqllite":
                    {
                        identityBuilder = builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<ApplicationSqlLiteIdentityDbContext>()
                            .AddSignInManager()
                            .AddDefaultTokenProviders();
                        break;
                    }
                case "npgsql":
                    {
                        identityBuilder = builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<ApplicationNpgsqlIdentityDbContext>()
                            .AddSignInManager()
                            .AddDefaultTokenProviders();
                        break;
                    }
                default:
                    {
                        identityBuilder = builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<ApplicationSqlServerIdentityDbContext>()
                            .AddSignInManager()
                            .AddDefaultTokenProviders();

                        break;
                    }
            }

            if (bool.Parse(builder.Configuration["Identity:Tokens:PersistKeysToDisk"] ?? "false"))
            {
                string dataDirectoryPath = builder.Configuration["DataProtection:Directory"] ?? string.Empty;

                if (!string.IsNullOrEmpty(dataDirectoryPath))
                {
                    identityBuilder.Services.AddDataProtection()
                        .PersistKeysToFileSystem(new DirectoryInfo(dataDirectoryPath));
                }
            }

            // If Environment is Docker, then use Redis Cache
            if (builder.Environment.IsDocker())
            {
                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = builder.Configuration.GetConnectionString("CACHE");
                });
            }
            else
            {
                builder.Services.AddDistributedMemoryCache();
            }

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

            builder.Services.AddAuthorization(options =>
            {
                options.AddIsAdminPolicy();
            });

            //Manage Serilog
            var serilogEnabled = bool.Parse(builder.Configuration["SerilogEnabled"] ?? "false");

            if (serilogEnabled)
            {
                // Configuration de Serilog
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration.GetSection("Serilog"))
                    .CreateLogger();

                builder.Logging.AddSerilog();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            Environment.Exit(1);
        }

        try
        {
            var app = builder.Build();

            //Load css
            if(!builder.Environment.IsDevelopment())
                StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration); //Add this

            // Call The migration of the identity database
            using var scope = app.Services.CreateScope();

            /************************************************************************************************
             * 1. Configure middleware pipeline
             * 2. configure Swagger
             * 3. Call Migration
             * 4. Seed the database with the admin account
             * 5. Configure Language settings
             * 6. Configure AccessDenied end point
             * 7. Set Culture from the URL
             * 8. Set Logout end point
             * 9. Use Authentication and Authorization middleware
            *************************************************************************************************/
            app.UseEndpointDefinitions();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            // Use Swagger
            app.ConfigureSwaggerUI(builder.Environment.ApplicationName);


            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            //TODO Identity 10 : Seed Admin account
            await SeedAdminAccount.SeedAdminUserAsync(userManager, roleManager, configuration);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsDocker())
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
            string[] supportedCultures = cultureService.GetSupportedCultures().ToArray();

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
            if (app.Environment.IsDevelopment() ||app.Environment.IsDocker())
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
                    context.Response.Redirect("/Account/AccessDenied");
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
                        new CookieOptions
                        {
                            Path = "/",
                            Expires = DateTimeOffset.Now.AddDays(14),
                            HttpOnly = true,
                            IsEssential = true,
                            SameSite = SameSiteMode.Lax
                        });
                }

                return Results.LocalRedirect(redirectUri);
            });

            app.MapGet("/Account/Logout", async (HttpContext httpContext, SignInManager<ApplicationUser> signInManager) =>
            {
                if (httpContext?.User?.Identity?.IsAuthenticated == true)
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
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            Environment.Exit(2);
        }
    }
}
