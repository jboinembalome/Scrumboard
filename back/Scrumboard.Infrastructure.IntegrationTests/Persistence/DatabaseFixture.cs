using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using Scrumboard.Infrastructure.Persistence;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence;

public class DatabaseFixture : IDisposable
{
    private const string DEFAULT_SQL_CONNECTION = "Server=(localdb)\\mssqllocaldb;Database=ScrumboardTestDb;Trusted_Connection=True;MultipleActiveResultSets=true;";
    private readonly Mock<IDateTime> _mockDateTime;
    
    public readonly Mock<ICurrentUserService> MockCurrentUserService;
    public ScrumboardDbContext DbContext { get; private set; }

    public DatabaseFixture()
    {
        var currentUserService = Guid.Parse("00000000-0000-0000-0000-000000000000");
        MockCurrentUserService = new Mock<ICurrentUserService>();
        MockCurrentUserService.Setup(m => m.UserId).Returns(currentUserService);

        _mockDateTime = new Mock<IDateTime>();
        _mockDateTime.Setup(m => m.Now).Returns(DateTime.Now);
    }  

    public IAsyncRepository<T, TId> GetRepository<T, TId>() where T : class, IEntity<TId>
    {      
        SetDbContext();

        return new BaseRepository<T,TId>(DbContext!);
    }
    
    public async Task ResetState()
    {
        var checkpoint = await Respawner.CreateAsync(DEFAULT_SQL_CONNECTION, new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory"]
        });

        await checkpoint.ResetAsync(DEFAULT_SQL_CONNECTION);
    }

    private static DbContextOptions<ScrumboardDbContext> CreateNewContextOptions()
    {
        DbContextOptionsBuilder<ScrumboardDbContext> builder;
        
        // Create a fresh service provider, and therefore a fresh
        // Sql Server database instance.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

        builder = new DbContextOptionsBuilder<ScrumboardDbContext>();
        builder.UseSqlServer(DEFAULT_SQL_CONNECTION)
            .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }

    public void SetDbContext()
    {
        var options = CreateNewContextOptions();
        
        DbContext = new ScrumboardDbContext(options, MockCurrentUserService.Object, _mockDateTime.Object);
        DbContext.Database.Migrate();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }
}
