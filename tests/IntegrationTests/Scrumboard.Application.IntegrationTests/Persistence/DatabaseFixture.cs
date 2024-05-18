using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Respawn;
using Scrumboard.Infrastructure.Persistence;
using Scrumboard.Infrastructure.Persistence.Repositories;
using System;
using System.Threading.Tasks;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.IntegrationTests.Persistence;

public class DatabaseFixture : IDisposable
{
    private const string DEFAULT_SQL_CONNECTION = "Server=(localdb)\\mssqllocaldb;Database=ScrumboardTestDb;Trusted_Connection=True;MultipleActiveResultSets=true;";
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<IDateTime> _mockDateTime;

    public ScrumboardDbContext DbContext { get; private set; }

    public DatabaseFixture()
    {
        var _currentUserService = "00000000-0000-0000-0000-000000000000";
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockCurrentUserService.Setup(m => m.UserId).Returns(_currentUserService);

        _mockDateTime = new Mock<IDateTime>();
        _mockDateTime.Setup(m => m.Now).Returns(DateTime.Now);          
    }  

    public IAsyncRepository<T, TId> GetRepository<T, TId>(bool inMemoryDatabase = false) where T : class, IEntity<TId>
    {      
        if (DbContext == null)
            SetDbContext(inMemoryDatabase);

        return new BaseRepository<T,TId>(DbContext);
    }

    public static async Task ResetState()
    {
        Checkpoint checkpoint = new()
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory" }
        };

        await checkpoint.Reset(DEFAULT_SQL_CONNECTION);
    }

    private static DbContextOptions<ScrumboardDbContext> CreateNewContextOptions(bool inMemoryDatabase)
    {
        DbContextOptionsBuilder<ScrumboardDbContext> builder;

        if (inMemoryDatabase)
        {
            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            builder = new DbContextOptionsBuilder<ScrumboardDbContext>();
            builder.UseInMemoryDatabase("ScrumboardTestDb")
                .UseInternalServiceProvider(serviceProvider);
        }
        else
        {
            // Create a fresh service provider, and therefore a fresh
            // Sql Server database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            builder = new DbContextOptionsBuilder<ScrumboardDbContext>();
            builder.UseSqlServer(DEFAULT_SQL_CONNECTION)
                .UseInternalServiceProvider(serviceProvider);
        }

        return builder.Options;
    }

    public void SetDbContext(bool inMemoryDatabase = false)
    {
        var options = CreateNewContextOptions(inMemoryDatabase);
        var operationalStoreOptions = Options.Create(new OperationalStoreOptions());

        DbContext = new ScrumboardDbContext(options, operationalStoreOptions, _mockCurrentUserService.Object, _mockDateTime.Object);
        if (!inMemoryDatabase)
            DbContext.Database.Migrate();
        else
            DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }
}