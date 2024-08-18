using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Infrastructure.Persistence;

public abstract class AuditableEntityTypeConfiguration<TEntity> 
    : IEntityTypeConfiguration<TEntity> 
    where TEntity : class, IEntity, ICreatedAtEntity, IModifiedAtEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(x => x.CreatedBy)
            .HasMaxLength(36)
            .HasConversion(
                x => (string)x,
                x => x);
        
        builder.Property(x => x.LastModifiedBy)
            .HasMaxLength(36)
            .HasConversion(
                x => (string?)x!,
                x =>x);
        
        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}
