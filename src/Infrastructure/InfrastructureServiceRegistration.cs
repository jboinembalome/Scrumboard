using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrumboard.Application.Interfaces.Common;
using Scrumboard.Application.Interfaces.FileExport;
using Scrumboard.Application.Interfaces.Identity;
using Scrumboard.Application.Interfaces.Logging;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Infrastructure.Common;
using Scrumboard.Infrastructure.FileExport;
using Scrumboard.Infrastructure.Identity;
using Scrumboard.Infrastructure.Logging;
using Scrumboard.Infrastructure.Persistence;
using Scrumboard.Infrastructure.Persistence.Repositories;

namespace Scrumboard.Infrastructure
{
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
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ScrumboardDbContext).Assembly.FullName)));
            }

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ScrumboardDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ScrumboardDbContext>();

            services.AddTransient<IProfileService, ProfileService>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient(typeof(ICsvExporter<>), typeof(CsvExporter<>));

            services.AddScoped(typeof(IAsyncRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.AddAuthentication()
                .AddIdentityServerJwt();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            //});

            return services;
        }
    }
}
