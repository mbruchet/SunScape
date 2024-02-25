using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SunScape.Options;

namespace SunScape.Services;

public class SmtpEmailService : IEmailSender
{
    private SmtpEmailServiceSettings _settings;

    public SmtpEmailService(IOptions<SmtpEmailServiceSettings> settings)
    {
        _settings = settings.Value;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var mailMessage = new MailMessage()
        {
            From = new MailAddress(GlobalConstats.EmailSender),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        using (var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort))
        {
            if (!string.IsNullOrEmpty(_settings.UserName))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_settings.UserName, _settings.Password);
            }

            client.EnableSsl = _settings.EnableSsl;
            client.Send(mailMessage);
        }

        return Task.CompletedTask;
    }
}