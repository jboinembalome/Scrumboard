using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Infrastructure.Persistence.Configurations
{
    public class LabelConfiguration : IEntityTypeConfiguration<Label>
    {
        public void Configure(EntityTypeBuilder<Label> builder)
        {
            builder.Property(l => l.Name)
                .IsRequired();

            builder
                .OwnsOne(l => l.Colour);
        }
    }
}
