using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using static SunScape.Components.Account.Pages.Login;
using SunScape.Data;

namespace SunScape.Components.Account.Pages.Manage;

public class ExternalLoginModel
{
    public UserLoginInfo LoginInfo { get; set; }
    public bool IsSelected { get; set; }
}

public partial class ExternalLogins
{
    public const string LinkLoginCallbackAction = "LinkLoginCallback";
    public const string ReturnUrl = "/Account/Manage/ExternalLogins";

    private ApplicationUser user = default!;

    [SupplyParameterFromForm]
    private IList<ExternalLoginModel>? currentLogins { get; set; }

    private IList<AuthenticationScheme>? otherLogins;

    [Inject]
    private IHttpContextAccessor HttpContextAcc { get; set; } = default!;

    [SupplyParameterFromQuery]
    private string? Action { get; set; }

    private List<ExternalProvider> externalLogins = new();

    protected override async Task OnInitializedAsync()
    {
        if (HttpContextAcc?.HttpContext != null)
        {
            user = await UserAccessor.GetRequiredUserAsync(HttpContextAcc.HttpContext);
            currentLogins = (await UserManager.GetLoginsAsync(user))?.Select(l=>new ExternalLoginModel { LoginInfo = l }).ToList() ?? new();
            otherLogins = (await SignInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => currentLogins.All(ul => auth.Name != ul.LoginInfo.LoginProvider))
                .ToList();

            string? passwordHash = null;
            if (UserStore is IUserPasswordStore<ApplicationUser> userPasswordStore)
            {
                passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContextAcc.HttpContext.RequestAborted);
            }

            if (HttpMethods.IsGet(HttpContextAcc.HttpContext.Request.Method) && Action == LinkLoginCallbackAction)
            {
                await OnGetLinkLoginCallbackAsync();
            }

            var externalProviders = await SignInManager.GetExternalAuthenticationSchemesAsync();

            externalLogins = externalProviders.Select(s => new ExternalProvider
            {
                Name = s.Name,
                DisplayName = s.DisplayName,
                IconPath = $"/icons/acount/icon-{s.Name.ToLower()}.png"
            }).ToList();
        }
    }

    async Task OnSubmit()
    {
        var selectedLogins = currentLogins.Where(x => x.IsSelected).ToList();

        bool success = true;

        foreach (var loginModel in selectedLogins)
        {
            var result = await UserManager.RemoveLoginAsync(user, loginModel.LoginInfo.LoginProvider, loginModel.LoginInfo.ProviderKey);
            success = result.Succeeded;

            if (!success)
                break;
        }

        if (!success)
        {
            RedirectManager.RedirectToCurrentPageWithStatus("Error: The external login was not removed.", HttpContextAcc.HttpContext);
        }
        else
        {
            NavigationManager.Refresh(true);
        }
    }

    private async Task OnGetLinkLoginCallbackAsync()
    {
        var userId = await UserManager.GetUserIdAsync(user);
        var info = await SignInManager.GetExternalLoginInfoAsync(userId);
        if (info is null)
        {
            RedirectManager.RedirectToCurrentPageWithStatus("Error: Could not load external login info.", HttpContextAcc.HttpContext);
        }

        var result = await UserManager.AddLoginAsync(user, info);
        if (!result.Succeeded)
        {
            RedirectManager.RedirectToCurrentPageWithStatus("Error: The external login was not added. External logins can only be associated with one account.", HttpContextAcc.HttpContext);
        }

        // Clear the existing external cookie to ensure a clean login process
        await HttpContextAcc.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        RedirectManager.RedirectToCurrentPageWithStatus("The external login was added.", HttpContextAcc.HttpContext);
    }
}
