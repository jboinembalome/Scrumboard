using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Persistence.Adherents;

internal sealed class AdherentConfiguration : IEntityTypeConfiguration<Adherent>
{
    public void Configure(EntityTypeBuilder<Adherent> builder)
    {
        builder.Property(a => a.IdentityId)
            .IsRequired();

        builder.HasMany(x => x.Cards)
            .WithMany(x => x.Adherents)
            .UsingEntity<Dictionary<string, object>>(
                "IsMember",
                b => b.HasOne<Card>().WithMany().HasForeignKey("CardId"),
                b => b.HasOne<Adherent>().WithMany().HasForeignKey("AdherentId"));
    }
}