using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards.Checklists;

namespace Scrumboard.Infrastructure.Persistence.Cards.Checklists;

public class ChecklistConfiguration : IEntityTypeConfiguration<Checklist>
{
    public void Configure(EntityTypeBuilder<Checklist> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired();
    }
}