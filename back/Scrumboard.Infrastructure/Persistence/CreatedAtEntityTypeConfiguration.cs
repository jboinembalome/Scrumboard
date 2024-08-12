using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Infrastructure.Persistence;

public abstract class CreatedAtEntityTypeConfiguration<TCreatedAtEntity, TEntityId> 
    : IEntityTypeConfiguration<TCreatedAtEntity> 
    where TEntityId : struct, IEquatable<TEntityId>
    where TCreatedAtEntity : class, ICreatedAtEntity 
{
    public void Configure(EntityTypeBuilder<TCreatedAtEntity> builder)
    {
        builder.Property(x => x.CreatedBy)
            .HasConversion(
                x => (string)x,
                x => (UserId)x);
        
        ConfigureDetails(builder);
    }

    protected abstract void ConfigureDetails(EntityTypeBuilder<TCreatedAtEntity> builder);
}
