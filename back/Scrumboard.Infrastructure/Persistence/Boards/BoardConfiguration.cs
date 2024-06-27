using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardConfiguration : IEntityTypeConfiguration<Board>
{
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.Property(b => b.Name)
            .IsRequired();

        builder.Property(b => b.Uri)
            .IsRequired();
        
        // TODO: Use Owned entity instead?
        builder
            .HasOne(x => x.BoardSetting)
            .WithOne()
            .HasForeignKey<BoardSetting>(x => x.BoardId);

        builder
            .HasOne(x => x.Team)
            .WithMany();
        
        builder
            .HasMany(x => x.ListBoards)
            .WithOne()
            .HasForeignKey(x => x.BoardId);
    }
}
