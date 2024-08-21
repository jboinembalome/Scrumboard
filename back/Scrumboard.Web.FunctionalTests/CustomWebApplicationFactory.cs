using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Persistence;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.FunctionalTests;

public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Startup>, IAsyncLifetime
{
    private readonly Database _database;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    
    private IServiceScopeFactory _scopeFactory;
    
    public CustomWebApplicationFactory()
    {
        _database = new Database();
        
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        InitializeCurrentUser("533f27ad-d3e8-4fe7-9259-ee4ef713dbea");
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseTestServer()
            .ConfigureTestServices(services =>
            {
                // Remove the ICurrentUserService registration present in Startup
                // And add a new ICurrentUserService registration.
                services
                    .RemoveAll<ICurrentUserService>()
                    .AddSingleton(_currentUserServiceMock.Object);
                
                // Remove the ScrumboardDbContext registration present in Startup
                // And add a new ScrumboardDbContext registration.
                services
                    .RemoveAll<DbContextOptions<ScrumboardDbContext>>()
                    .AddDbContext<ScrumboardDbContext>((serviceProvider, options) =>
                    {
                        options
                            .UseSqlServer(
                                _database.GetConnectionString(), 
                                b => b.MigrationsAssembly(typeof(ScrumboardDbContext).Assembly.FullName))
                            .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
                    });
            });
    }

    public Task ResetDatabaseAsync()
        => _database.ResetAsync();
    
    public async Task InitializeAsync()
    {
        await _database.TryDeleteAsync();
        await _database.CreateAsync();
        
        _scopeFactory = Services.GetRequiredService<IServiceScopeFactory>();
    }
    
    public async Task<TEntity> AddEntityAsync<TEntity>(TEntity entity)
        where TEntity : class, IEntity
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ScrumboardDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();

        return entity;
    }

    async Task IAsyncLifetime.DisposeAsync() 
        => await _database.DisposeAsync();

    private void InitializeCurrentUser(string userId) 
        => _currentUserServiceMock
            .Setup(m => m.UserId)
            .Returns(userId);
}
