using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.SharedKernel.Entities;

namespace Scrumboard.Infrastructure.Persistence;

public abstract class CreatedAtEntityTypeConfiguration<TEntity, TEntityId> 
    : IEntityTypeConfiguration<TEntity> 
    where TEntityId : notnull
    where TEntity : class, IEntity, ICreatedAtEntity 
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(x => x.CreatedBy)
            .HasConversion(
                x => (string)x,
                x => x);
        
        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}
