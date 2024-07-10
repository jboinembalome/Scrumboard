using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        
        // No foreign key to avoid SQL Server error:
        // Introducing FOREIGN KEY constraint 'FK_Labels_Boards_BoardId' on table 'Labels'
        // may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION,
        // or modify other FOREIGN KEY constraints.
        builder.Property(l => l.BoardId)
            .IsRequired();
        
        builder.HasIndex(x => new { x.Id, x.BoardId })
            .IsUnique();
    }
}
