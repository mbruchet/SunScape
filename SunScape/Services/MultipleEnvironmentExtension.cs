namespace SunScape.Services;

public static class MultipleEnvironmentExtension
{
    public static bool IsDocker(this IHostEnvironment env)
    {
        var dockerRuningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

        if(!string.IsNullOrEmpty(dockerRuningInContainer))
        {
            return bool.Parse(dockerRuningInContainer);
        }

        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if(!string.IsNullOrEmpty(environmentName))
        {
            return environmentName.Equals("Docker", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    public static bool IsAzure(this IHostEnvironment env)
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if(!string.IsNullOrEmpty(environmentName))
        {
            return environmentName.Equals("Azure", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}
