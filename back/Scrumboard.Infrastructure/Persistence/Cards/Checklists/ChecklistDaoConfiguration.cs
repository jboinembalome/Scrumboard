using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Cards.Checklists;

internal sealed class ChecklistDaoConfiguration : IEntityTypeConfiguration<ChecklistDao>
{
    public void Configure(EntityTypeBuilder<ChecklistDao> builder)
    {
        builder.ToTable("Checklists");
        
        builder.Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder
            .HasMany(x => x.ChecklistItems)
            .WithOne()
            .HasForeignKey(x => x.ChecklistId);
    }
}
