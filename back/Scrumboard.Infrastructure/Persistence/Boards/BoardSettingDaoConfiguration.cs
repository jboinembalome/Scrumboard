using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardSettingDaoConfiguration : IEntityTypeConfiguration<BoardSettingDao>
{
    public void Configure(EntityTypeBuilder<BoardSettingDao> builder)
    {
        builder.ToTable("BoardSettings");
        
        builder.Property(b => b.CardCoverImage)
            .IsRequired();

        builder.Property(b => b.Subscribed)
            .IsRequired();

        builder
            .OwnsOne(b => b.Colour);
    }
}
