using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Boards.ListBoards;

internal sealed class ListBoardDaoConfiguration : IEntityTypeConfiguration<ListBoardDao>
{
    public void Configure(EntityTypeBuilder<ListBoardDao> builder)
    {
        builder.ToTable("ListBoards");
        
        builder.Property(l => l.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder
            .HasMany(x => x.Cards)
            .WithOne()
            .HasForeignKey(x => x.ListBoardId);
    }
}
