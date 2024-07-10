using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Infrastructure.Persistence.Boards;

namespace Scrumboard.Infrastructure.Persistence.Cards.Labels;

internal sealed class LabelDaoConfiguration : IEntityTypeConfiguration<LabelDao>
{
    public void Configure(EntityTypeBuilder<LabelDao> builder)
    {
        builder.ToTable("Labels");

        builder
            .OwnsOne(l => l.Colour);
        
        builder.Property(l => l.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.HasOne<BoardDao>()
            .WithMany()
            .HasForeignKey(x => x.BoardId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
