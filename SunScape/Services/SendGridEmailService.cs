using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SunScape.Options;
using System.Net.Mail;

namespace SunScape.Services;

public class SendGridEmailService : IEmailSender
{
    private SendGridEmailServiceSettings _settings;

    public SendGridEmailService(IOptions<SendGridEmailServiceSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SendGridClient(_settings.ApiKey);
        var from = new EmailAddress(GlobalConstats.EmailSender, GlobalConstats.EmailSenderName);
        var to = new EmailAddress(email);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
        await client.SendEmailAsync(msg);
    }
}