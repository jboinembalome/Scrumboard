using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.FileExport;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Logging;
using Scrumboard.Infrastructure.Abstractions.Persistence;
using Scrumboard.Infrastructure.Common;
using Scrumboard.Infrastructure.FileExport;
using Scrumboard.Infrastructure.Identity;
using Scrumboard.Infrastructure.Logging;
using Scrumboard.Infrastructure.Persistence;

namespace Scrumboard.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ScrumboardDbContext>(options =>
                    options.UseInMemoryDatabase("ScrumboardDb"));
            }
            else
            {
                services.AddDbContext<ScrumboardDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }

            services
                .AddIdentityApiEndpoints<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ScrumboardDbContext>();
        
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient(typeof(ICsvExporter<>), typeof(CsvExporter<>));

            services.AddScoped(typeof(IAsyncRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.AddAuthentication();
            
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            //});

            return services;
        }
}
