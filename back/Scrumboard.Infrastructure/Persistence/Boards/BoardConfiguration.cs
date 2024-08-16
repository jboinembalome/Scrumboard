using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardConfiguration : AuditableEntityTypeConfiguration<Board>, IModelConfiguration
{
    private const string TableName = "Boards";
    
    protected override void ConfigureEntity(EntityTypeBuilder<Board> builder)
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
        
        builder.Property(b => b.OwnerId)
            .HasConversion(
                x => (string)x,
                x => new OwnerId(x))
            .HasMaxLength(36)
            .IsRequired();
    }

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSequence<Board, BoardId>(increment: 50);
    }
}
