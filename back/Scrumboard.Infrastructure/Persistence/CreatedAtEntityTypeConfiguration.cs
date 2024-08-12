using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Infrastructure.Persistence;

public abstract class CreatedAtEntityTypeConfiguration<TEntity, TEntityId> 
    : IEntityTypeConfiguration<TEntity> 
    where TEntityId : struct, IEquatable<TEntityId>
    where TEntity : class, ICreatedAtEntity 
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(x => x.CreatedBy)
            .HasConversion(
                x => (string)x,
                x => (UserId)x);
        
        ConfigureDetails(builder);
    }

    protected abstract void ConfigureDetails(EntityTypeBuilder<TEntity> builder);
}
