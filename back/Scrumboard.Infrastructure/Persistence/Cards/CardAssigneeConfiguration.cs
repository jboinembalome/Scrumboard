using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Infrastructure.Persistence.Cards;

internal sealed class CardAssigneeConfiguration : IEntityTypeConfiguration<CardAssignee>
{
    public void Configure(EntityTypeBuilder<CardAssignee> builder)
    {
        builder.ToTable("CardAssignees");

        builder.HasKey(x => new { x.CardId, x.AssigneeId });

        builder.Property(x => x.AssigneeId)
            .HasMaxLength(36);
        
        builder.Property(x => x.CardId)
            .HasConversion(
                x => (int)x,
                x => (CardId)x);
        
        builder.Property(x => x.AssigneeId)
            .HasConversion(
                x => (string)x,
                x => (UserId)x);
    }
}
