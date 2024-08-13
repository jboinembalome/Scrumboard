using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardConfiguration : AuditableEntityTypeConfiguration<Board, BoardId>, IModelConfiguration
{
    private const string TableName = "Boards";
    
    protected override void ConfigureDetails(EntityTypeBuilder<Board> builder)
    {
        builder.ToTable(TableName);
        
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
            .HasConversion(
                x => (int)x,
                x => new BoardId(x));
    }

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSequence<Board, BoardId>(increment: 50);
    }
}
