using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Cards.Labels;

internal sealed class LabelDaoConfiguration : IEntityTypeConfiguration<LabelDao>
{
    public void Configure(EntityTypeBuilder<LabelDao> builder)
    {
        builder.ToTable("Labels");
        
        builder.Property(l => l.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .OwnsOne(l => l.Colour);
    }
}
