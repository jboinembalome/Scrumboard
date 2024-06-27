using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(c => c.Message)
            .IsRequired();
    }
}
