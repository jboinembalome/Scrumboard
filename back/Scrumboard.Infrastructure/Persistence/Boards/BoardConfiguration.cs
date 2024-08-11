using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardConfiguration : AuditableEntityTypeConfiguration<Board, BoardId>
{
    protected override void ConfigureDetails(EntityTypeBuilder<Board> builder)
    {
        builder.ToTable("Boards");
        
        builder
            .HasKey(x => x.Id);
        
        builder.Property(b => b.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder
            .HasOne(x => x.BoardSetting)
            .WithOne()
            .HasForeignKey<BoardSetting>(x => x.BoardId);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(
                x => (int)x,
                x => new BoardId(x));
    }
}
