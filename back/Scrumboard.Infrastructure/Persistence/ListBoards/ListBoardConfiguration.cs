using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Persistence.ListBoards;

internal sealed class ListBoardConfiguration : AuditableEntityTypeConfiguration<ListBoard>, IModelConfiguration
{
    private const string TableName = "ListBoards";
    
    protected override void ConfigureEntity(EntityTypeBuilder<ListBoard> builder)
    {
        builder.ToTable(TableName);
        
        builder
            .HasKey(x => x.Id);
        
        builder.Property(l => l.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder
            .HasMany(x => x.Cards)
            .WithOne()
            .HasForeignKey(x => x.ListBoardId);
        
        builder
            .HasOne<Board>()
            .WithMany()
            .HasForeignKey(x => x.BoardId);
        
        builder.Property(x => x.Id)
            .HasConversion(
                x => (int)x,
                x => (ListBoardId)x);
        
        builder.Property(x => x.BoardId)
            .HasConversion(
                x => (int)x,
                x => (BoardId)x);
    }

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSequence<ListBoard, ListBoardId>(increment: 50);
    }
}
