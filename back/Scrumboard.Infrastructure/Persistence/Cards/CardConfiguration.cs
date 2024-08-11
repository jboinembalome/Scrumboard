using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Persistence.Cards.Activities;
using Scrumboard.Infrastructure.Persistence.Cards.Comments;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardConfiguration : AuditableEntityTypeConfiguration<Card, CardId>
{
    protected override void ConfigureDetails(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("Cards");
        
        builder
            .HasKey(x => x.Id);
        
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
            .HasMany(x => x.Labels)
            .WithOne()
            .HasForeignKey(x => x.CardId);
        
        builder
            .HasMany<Comment>()
            .WithOne()
            .HasForeignKey(x => x.CardId);
        
        builder
            .HasMany<Activity>()
            .WithOne()
            .HasForeignKey(x => x.CardId);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(
                x => (int)x,
                x => (CardId)x);
        
        builder.Property(x => x.ListBoardId)
            .HasConversion(
                x => (int)x,
                x => (ListBoardId)x);
    }
}
