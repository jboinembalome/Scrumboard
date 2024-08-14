using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentConfiguration : CreatedAtEntityTypeConfiguration<Comment, CommentId>, IModelConfiguration
{
    private const string TableName = "Comments";
    
    protected override void ConfigureEntity(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(x => x.Id);
        
        builder.Property(c => c.Message)
            .HasMaxLength(1000)
            .IsRequired();
        
        builder.Property(x => x.Id)
            .HasConversion(
                x => (int)x,
                x => (CommentId)x);
        
        builder.Property(x => x.CardId)
            .HasConversion(
                x => (int)x,
                x => (CardId)x);
    }

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSequence<Comment, CommentId>(increment: 50);
    }
}
