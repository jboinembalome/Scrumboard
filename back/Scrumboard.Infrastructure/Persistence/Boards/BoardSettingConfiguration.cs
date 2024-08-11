using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardSettingConfiguration : IEntityTypeConfiguration<BoardSetting>
{
    public void Configure(EntityTypeBuilder<BoardSetting> builder)
    {
        builder.ToTable("BoardSettings");

        builder
            .HasKey(x => x.Id);
        
        builder
            .OwnsOne(b => b.Colour);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(
                x => (int)x,
                x => (BoardSettingId)x);
    }
}
