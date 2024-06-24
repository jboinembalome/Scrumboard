using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired();
        
        builder
            .HasMany(x => x.Labels)
            .WithMany();

        builder
            .HasMany(x => x.Assignees)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "CardAssignees",
                b => b.HasOne<Adherent>().WithMany().HasForeignKey("AdherentId"),
                b => b.HasOne<Card>().WithMany().HasForeignKey("CardId"));
    }
}
