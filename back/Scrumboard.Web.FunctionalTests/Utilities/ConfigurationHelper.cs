using Microsoft.Extensions.Configuration;

namespace Scrumboard.Web.FunctionalTests.Utilities;

internal static class ConfigurationHelper
{
    private static readonly IConfigurationRoot ConfigurationBuilder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    
    public static IConfigurationRoot GetConfiguration() 
        => ConfigurationBuilder;
}
