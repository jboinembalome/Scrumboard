using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardAssigneeDaoConfiguration : IEntityTypeConfiguration<CardAssigneeDao>
{
    public void Configure(EntityTypeBuilder<CardAssigneeDao> builder)
    {
        builder.ToTable("CardAssignees");

        builder.HasKey(x => new { x.CardId, x.AssigneeId });

        builder.Property(x => x.AssigneeId)
            .HasMaxLength(36);
    }
}
