using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardConfiguration : AuditableEntityTypeConfiguration<Card>, IModelConfiguration
{
    private const string TableName = "Cards";
    
    protected override void ConfigureEntity(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable(TableName);
        
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
            .HasConversion(
                x => (int)x,
                x => x);
        
        builder.Property(x => x.ListBoardId)
            .HasConversion(
                x => (int)x,
                x => x);
    }

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSequence<Card, CardId>(increment: 50);
    }
}
