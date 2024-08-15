using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using Scrumboard.Infrastructure.Persistence;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Persistence.Interceptors;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence;

public sealed class DatabaseFixture : IDisposable
{
    private const string DEFAULT_SQL_CONNECTION = "Server=(localdb)\\mssqllocaldb;Database=ScrumboardTestDb;Trusted_Connection=True;MultipleActiveResultSets=true;";
    
    private readonly DbContextOptions<ScrumboardDbContext> _dbContextOptions;
    private readonly ScrumboardDbContext _dbContext;

    private bool _disposed;
    
    public Mock<ICurrentUserService> CurrentUserServiceMock { get; }
    public Mock<ICurrentDateService> CurrentDateServiceMock { get; }
    
    public DatabaseFixture()
    {
        CurrentDateServiceMock = new Mock<ICurrentDateService>();
        InitializeCurrentDate(DateTimeOffset.Now);
        
        CurrentUserServiceMock = new Mock<ICurrentUserService>();
        InitializeCurrentUser("00000000-0000-0000-0000-000000000000");
        
        var serviceProvider = ConfigureServicesInternal()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();
        
        _dbContextOptions = GetDbContextOptions(serviceProvider);
        _dbContext = CreateDbContext();
    }
    
    public static async Task ResetState()
    {
        var checkpoint = await Respawner.CreateAsync(DEFAULT_SQL_CONNECTION, new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory"]
        });

        await checkpoint.ResetAsync(DEFAULT_SQL_CONNECTION);
    }
    
    public ScrumboardDbContext CreateDbContext()
    {
        var dbContext = new ScrumboardDbContext(_dbContextOptions);
        
        dbContext.Database.Migrate();
        
        return dbContext;
    }
    
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        _disposed = true;
    }
    
    private ServiceCollection ConfigureServicesInternal()
    {
        var services = new ServiceCollection();

        services.AddSingleton(CurrentUserServiceMock.Object);
        services.AddSingleton(CurrentDateServiceMock.Object);
        
        services.AddScoped<ISaveChangesInterceptor, CreatedEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, ModifiedEntityInterceptor>();
        
        return services;
    }
    
    private static DbContextOptions<ScrumboardDbContext> GetDbContextOptions(IServiceProvider serviceProvider) 
        => new DbContextOptionsBuilder<ScrumboardDbContext>()
            .UseSqlServer(DEFAULT_SQL_CONNECTION)
            .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>())
            .UseInternalServiceProvider(serviceProvider)
            .Options;
    
    private void InitializeCurrentDate(DateTimeOffset dateTimeOffset) 
        => CurrentDateServiceMock
            .Setup(m => m.Now)
            .Returns(dateTimeOffset);
    
    private void InitializeCurrentUser(string userId) 
        => CurrentUserServiceMock
            .Setup(m => m.UserId)
            .Returns(userId);
}
