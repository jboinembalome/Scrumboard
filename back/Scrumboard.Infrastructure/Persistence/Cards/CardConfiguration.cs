using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired();
        
        builder
            .HasMany(x => x.Checklists)
            .WithOne()
            .HasForeignKey(x => x.CardId);
        
        builder
            .HasMany(x => x.Labels)
            .WithMany()
            .UsingEntity("CardsLabels");
        
        builder
            .HasMany(x => x.Comments)
            .WithOne()
            .HasForeignKey(x => x.CardId);
        
        builder
            .HasMany(x => x.Activities)
            .WithOne()
            .HasForeignKey(x => x.CardId);
    }
}
