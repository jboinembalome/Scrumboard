using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Scrumboard.Infrastructure;
using Scrumboard.Infrastructure.Persistence;
using Scrumboard.Web.Services;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Scrumboard.Application;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Web.Api;
using Scrumboard.Web.ExceptionHandlers;
using Scrumboard.Web.Middlewares;

namespace Scrumboard.Web;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddApplicationServices();
        services.AddInfrastructureServices(_configuration);
        
        services.AddHttpContextAccessor();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddScoped<LoggingMiddleware>();

        services.AddHealthChecks()
            .AddDbContextCheck<ScrumboardDbContext>();

        services.AddControllersWithViews(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        });
        
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddExceptionHandler<UnhandledExceptionHandler>();
        
        services.AddProblemDetails();
        
        services.AddRazorPages();
        
        services.AddEndpointsApiExplorer();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        // In production, the Angular files will be served from this directory
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/dist";
        });
        
        AddSwagger(services);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void ConfigureMiddlewares(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        
        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        if (!env.IsDevelopment())
        {
            app.UseSpaStaticFiles();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scrumboard API");
        });

        app.UseRouting();

        app.UseMiddleware<LoggingMiddleware>();
        
        app.UseExceptionHandler();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });

        app.UseSpa(spa =>
        {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
                //spa.UseAngularCliServer(npmScript: "start");
                spa.UseProxyToSpaDevelopmentServer(_configuration["SpaBaseUrl"] ?? "http://localhost:4200");
            }
        });
    }

    private static void AddSwagger(IServiceCollection services)
    {
        // Register the Swagger generator, defining 1 or more Swagger documents
        services.AddSwaggerGen();

        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Scrumboard API",
                Description = "API for accessing Scrumboard project data.",
                Contact = new OpenApiContact
                {
                    Name = "Jimmy Boinembalome",
                    Email = "jboinembaome@gmail.com",
                    Url = new Uri("https://github.com/jboinembalome"),
                }
            });

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            //c.OperationFilter<FileResultContentTypeOperationFilter>();
        });
    }

}
