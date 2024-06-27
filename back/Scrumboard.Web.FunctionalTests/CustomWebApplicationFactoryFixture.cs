using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrumboard.Infrastructure.Identity;
using Scrumboard.Infrastructure.Persistence;

namespace Scrumboard.Web.FunctionalTests;

public class CustomWebApplicationFactoryFixture<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    private static IConfigurationRoot? _configuration;
    
    public CustomWebApplicationFactoryFixture()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = configurationBuilder.Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (_configuration != null)
        {
            builder.UseConfiguration(_configuration);

            builder.ConfigureServices(services =>
            {
                // Remove the ScrumboardDbContext registration present in Startup.
                var descriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ScrumboardDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Add a new ScrumboardDbContext registration.
                services.AddDbContext<ScrumboardDbContext>(options =>
                    options.UseSqlServer(
                        _configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ScrumboardDbContext).Assembly.FullName)));
                

                var serviceProvider = services.BuildServiceProvider();
                
                EnsureDatabase(serviceProvider);
            });
        }
    }

    private static void EnsureDatabase(ServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetService<ScrumboardDbContext>();

        context?.Database.Migrate();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        
        // TODO: Update EnsureDatabase according to ScrumboardDbContextSeed.InitialiseDatabaseAsync
        //await ScrumboardDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
        //await ScrumboardDbContextSeed.SeedSampleDataAsync(context);
    }
}
