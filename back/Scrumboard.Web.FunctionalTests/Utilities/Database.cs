using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Respawn;
using Scrumboard.Infrastructure.Persistence;

namespace Scrumboard.Web.FunctionalTests.Utilities;

internal sealed class Database
{
    private readonly string _connectionString;
    private readonly ScrumboardDbContext _dbContext;
    
    private Respawner _respawner = default!;

    public Database()
    {
        var configuration = ConfigurationHelper.GetConfiguration();
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;   
        
        var options = new DbContextOptionsBuilder<ScrumboardDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        _dbContext = new ScrumboardDbContext(options);
    }
    
    public string GetConnectionString() 
        => _connectionString;

    public async Task CreateAsync()
    {
        await _dbContext.Database.MigrateAsync();
        
        _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }
    
    public Task ResetAsync() 
        => _respawner.ResetAsync(_connectionString);

    public async Task TryDeleteAsync() 
        => await _dbContext.Database.EnsureDeletedAsync();

    public async Task DisposeAsync() 
        => await _dbContext.DisposeAsync();
}
