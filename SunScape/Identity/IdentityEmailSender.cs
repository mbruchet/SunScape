using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using SunScape.Data;

namespace SunScape.Identity
{
    public class IdentityEmailSender : IEmailSender<ApplicationUser>
    {
        private IEmailSender _emailSender;

        public IdentityEmailSender(Microsoft.AspNetCore.Identity.UI.Services.IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
            _emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
            _emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
            _emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");

    }
}
