using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardConfiguration : IEntityTypeConfiguration<BoardDao>
{
    public void Configure(EntityTypeBuilder<BoardDao> builder)
    {
        builder.ToTable("Boards");
        
        builder.Property(b => b.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder
            .HasOne(x => x.BoardSetting)
            .WithOne()
            .HasForeignKey<BoardSettingDao>(x => x.BoardId);

        builder
            .HasOne(x => x.Team)
            .WithMany();
        
        builder
            .HasMany(x => x.ListBoards)
            .WithOne()
            .HasForeignKey(x => x.BoardId);
    }
}
