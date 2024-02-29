using Microsoft.AspNetCore.Authorization;

namespace SunScape.Identity.Policies;

public static class IsAdminPolicy
{
    public const string PolicyName = "IsAdmin";

    public static AuthorizationOptions AddIsAdminPolicy(this AuthorizationOptions options)
    {
        options.AddPolicy(PolicyName, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole("Admin");
        });

        return options;
    }
}