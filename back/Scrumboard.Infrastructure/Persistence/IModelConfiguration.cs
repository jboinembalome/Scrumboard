using Microsoft.EntityFrameworkCore;

namespace Scrumboard.Infrastructure.Persistence;

internal interface IModelConfiguration
{
    void ConfigureModel(ModelBuilder modelBuilder);
}
