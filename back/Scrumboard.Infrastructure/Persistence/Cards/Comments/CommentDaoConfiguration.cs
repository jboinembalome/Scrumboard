using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentDaoConfiguration : IEntityTypeConfiguration<CommentDao>
{
    public void Configure(EntityTypeBuilder<CommentDao> builder)
    {
        builder.ToTable("Comments");
        
        builder.Property(c => c.Message)
            .HasMaxLength(1000)
            .IsRequired();
    }
}
