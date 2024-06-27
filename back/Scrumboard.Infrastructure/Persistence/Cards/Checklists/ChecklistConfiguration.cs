using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards.Checklists;

namespace Scrumboard.Infrastructure.Persistence.Cards.Checklists;

internal sealed class ChecklistConfiguration : IEntityTypeConfiguration<Checklist>
{
    public void Configure(EntityTypeBuilder<Checklist> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired();
        
        builder
            .HasMany(x => x.ChecklistItems)
            .WithOne()
            .HasForeignKey(x => x.ChecklistId);
    }
}
