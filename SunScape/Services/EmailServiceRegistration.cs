using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using SunScape.Data;
using SunScape.Identity;
using SunScape.Options;

namespace SunScape.Services
{
    public static class EmailServiceRegistration
    {
        public static IServiceCollection RegisterEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration["EmailSenderType"])
            {
                case "FileEmailService":
                    {
                        services.AddSingleton<IEmailSender, FileEmailService>();
                        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSender>();
                        break;
                    }
                case "SmtpEmailService":
                    {
                        services.Configure<SmtpEmailServiceSettings>(configuration.GetSection("EmailStmp"));
                        services.AddSingleton<IEmailSender, SmtpEmailService>();
                        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSender>();
                        break;
                    }
                case "SendGridEmailService":
                    {
                        services.Configure<SendGridEmailServiceSettings>(configuration.GetSection("SendGridEmailService"));
                        services.AddSingleton<IEmailSender, SendGridEmailService>();
                        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSender>();
                        break;
                    }
                default:
                    {
                        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
                        break;
                    }
            }

            return services;
        }
    }
}
