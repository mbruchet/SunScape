using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace SunScape.Services;

public class FileEmailService : IEmailSender
{
    private IConfiguration _configuration;
    private string? _emailFolderPath;

    public FileEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _emailFolderPath = _configuration["EmailFolderPath"] ?? "data/emails";

        if(!Directory.Exists(_emailFolderPath))
            Directory.CreateDirectory(_emailFolderPath);
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

        using (var client = new SmtpClient())
        {
            client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            client.PickupDirectoryLocation = _emailFolderPath;
            client.Send(mailMessage);
        }

        return Task.CompletedTask;
    }
}
