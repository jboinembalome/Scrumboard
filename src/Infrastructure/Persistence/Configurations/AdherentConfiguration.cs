using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Entities;
using System.Collections.Generic;

namespace Scrumboard.Infrastructure.Persistence.Configurations
{
    public class AdherentConfiguration : IEntityTypeConfiguration<Adherent>
    {
        public void Configure(EntityTypeBuilder<Adherent> builder)
        {
            builder.Property(a => a.IdentityGuid)
                .IsRequired();

            builder.HasMany(x => x.Cards)
                .WithMany(x => x.Adherents)
                .UsingEntity<Dictionary<string, object>>(
                  "IsMember",
                  b => b.HasOne<Card>().WithMany().HasForeignKey("CardId"),
                  b => b.HasOne<Adherent>().WithMany().HasForeignKey("AdherentId"));
        }
    }
}
