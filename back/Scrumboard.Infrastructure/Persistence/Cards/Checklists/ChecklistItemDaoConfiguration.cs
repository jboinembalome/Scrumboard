using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Cards.Checklists;

internal sealed class ChecklistItemDaoConfiguration : IEntityTypeConfiguration<ChecklistItemDao>
{
    public void Configure(EntityTypeBuilder<ChecklistItemDao> builder)
    {
        builder.ToTable("ChecklistItems");
        
        builder.Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired();
    }
}
