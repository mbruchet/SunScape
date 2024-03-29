using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace SunScape.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            //TODO 9. Register Localization side on client side
            builder.Services.AddLocalization(options => options.ResourcesPath = "Locales");

            await builder.Build().RunAsync();
        }
    }
}
