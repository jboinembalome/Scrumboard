using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Checklists;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Infrastructure.Persistence.Configurations
{
    public class ChecklistItemConfiguration : IEntityTypeConfiguration<ChecklistItem>
    {
        public void Configure(EntityTypeBuilder<ChecklistItem> builder)
        {
            builder.Property(c => c.Name)
              .IsRequired();
        }
    }
}
