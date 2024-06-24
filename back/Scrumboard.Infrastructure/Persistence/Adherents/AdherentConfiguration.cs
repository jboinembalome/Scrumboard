using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Adherents;

namespace Scrumboard.Infrastructure.Persistence.Adherents;

internal sealed class AdherentConfiguration : IEntityTypeConfiguration<Adherent>
{
    public void Configure(EntityTypeBuilder<Adherent> builder)
    {
        builder.Property(a => a.IdentityId)
            .IsRequired();
    }
}
