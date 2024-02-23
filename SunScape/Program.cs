using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using SunScape.Client.Pages;
using SunScape.Components;
using SunScape.Services;

namespace SunScape
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // TODO 2. Register a local service to list cultures 
            builder.Services.AddSingleton<CultureService>();

            /// TODO 3. Add Localization middleware with folder path
            builder.Services.AddLocalization(options=>options.ResourcesPath = "Locales");

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();


            var app = builder.Build();

            // TODO 4. Get culture service instance from DI
            var cultureService = app.Services.GetRequiredService<CultureService>();

            // TODO 5. Get supported cultures
            string[] supportedCultures = cultureService.GetSupportedCultures().ToArray() ;

            // TODO 6. Define supported cultures
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            // TODO 7. Add on first position cookie detection 
            localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

            // TODO 8. Use request localization
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
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

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
            
            app.Run();
        }
    }
}
