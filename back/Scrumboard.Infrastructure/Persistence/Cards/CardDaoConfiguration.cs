using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Infrastructure.Persistence.Cards.Activities;
using Scrumboard.Infrastructure.Persistence.Cards.Comments;
using Scrumboard.Infrastructure.Persistence.Cards.Labels;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardDaoConfiguration : IEntityTypeConfiguration<CardDao>
{
    public void Configure(EntityTypeBuilder<CardDao> builder)
    {
        builder.ToTable("Cards");
        
        builder.Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(1000);
        
        builder
            .HasMany(x => x.Assignees)
            .WithOne()
            .HasForeignKey(x => x.CardId);
            
        builder
            .HasMany(x => x.Checklists)
            .WithOne()
            .HasForeignKey(x => x.CardId);
        
        builder
            .HasMany(x => x.Labels)
            .WithMany()
            .UsingEntity(
                "CardsLabels",
                l => l
                    .HasOne(typeof(LabelDao))
                    .WithMany()
                    .HasForeignKey("LabelId"),
                r => r
                    .HasOne(typeof(CardDao))
                    .WithMany()
                    .HasForeignKey("CardId"),
                j => j.HasKey("CardId", "LabelId"));
        
        builder
            .HasMany<CommentDao>()
            .WithOne()
            .HasForeignKey(x => x.CardId);
        
        builder
            .HasMany<ActivityDao>()
            .WithOne()
            .HasForeignKey(x => x.CardId);
    }
}
