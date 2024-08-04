using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using Scrumboard.Infrastructure.Persistence;
using Scrumboard.Infrastructure.Abstractions.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence;

public sealed class DatabaseFixture : IDisposable
{
    private const string DEFAULT_SQL_CONNECTION = "Server=(localdb)\\mssqllocaldb;Database=ScrumboardTestDb;Trusted_Connection=True;MultipleActiveResultSets=true;";
    
    private readonly ICurrentDateService _currentDateService;
    private readonly ICurrentUserService _currentUserService;
    
    public ScrumboardDbContext DbContext { get; private set; }

    public DatabaseFixture()
    {
        _currentUserService = Mock.Of<ICurrentUserService>();
        SetupCurrentUserService();

        _currentDateService = Mock.Of<ICurrentDateService>();
        SetupCurrentDateService();

        SetupDbContext();
    }
    
    public static async Task ResetState()
    {
        var checkpoint = await Respawner.CreateAsync(DEFAULT_SQL_CONNECTION, new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory"]
        });

        await checkpoint.ResetAsync(DEFAULT_SQL_CONNECTION);
    }
    
    private void SetupDbContext()
    {
        var options = CreateNewContextOptions();
        
        DbContext = new ScrumboardDbContext(options, _currentUserService, _currentDateService);
        DbContext.Database.Migrate();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }
    
    private void SetupCurrentDateService() 
        => Mock.Get(_currentDateService)
            .Setup(m => m.Now)
            .Returns(DateTimeOffset.Now);

    private void SetupCurrentUserService() 
        => Mock.Get(_currentUserService)
            .Setup(m => m.UserId)
            .Returns("00000000-0000-0000-0000-000000000000");
    
    private static DbContextOptions<ScrumboardDbContext> CreateNewContextOptions()
    {
        // Create a fresh service provider, and therefore a fresh
        // Sql Server database instance.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

        var builder = new DbContextOptionsBuilder<ScrumboardDbContext>();
        
        builder
            .UseSqlServer(DEFAULT_SQL_CONNECTION)
            .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }
}
