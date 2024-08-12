using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Infrastructure.Persistence;

public abstract class AuditableEntityTypeConfiguration<TAuditableEntity, TEntityId> 
    : IEntityTypeConfiguration<TAuditableEntity> 
    where TEntityId : struct, IEquatable<TEntityId>
    where TAuditableEntity : class, ICreatedAtEntity, IModifiedAtEntity
{
    public void Configure(EntityTypeBuilder<TAuditableEntity> builder)
    {
        builder.Property(x => x.CreatedBy)
            .HasMaxLength(36)
            .HasConversion(
                x => (string)x,
                x => (UserId)x);
        
        builder.Property(x => x.LastModifiedBy)
            .HasMaxLength(36)
            .HasConversion(
                x => (string?)x,
                x =>(UserId?)x!);
        
        ConfigureDetails(builder);
    }

    protected abstract void ConfigureDetails(EntityTypeBuilder<TAuditableEntity> builder);
}
