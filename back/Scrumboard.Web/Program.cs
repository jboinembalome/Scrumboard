using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Infrastructure.Identity;
using Scrumboard.Infrastructure.Persistence;
using Scrumboard.Web;


var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Use Identity API for simplicity.
    // TODO: Use Keycloak with Oauth2.
    app.InitialiseIdentityApi(); 
    
    await app.InitialiseDatabaseAsync();
}

startup.ConfigureMiddlewares(app, app.Environment);

await app.RunAsync();







