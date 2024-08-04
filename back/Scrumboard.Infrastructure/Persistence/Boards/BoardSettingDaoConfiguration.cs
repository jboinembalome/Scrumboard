using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardSettingDaoConfiguration : IEntityTypeConfiguration<BoardSettingDao>
{
    public void Configure(EntityTypeBuilder<BoardSettingDao> builder)
    {
        builder.ToTable("BoardSettings");

        builder
            .OwnsOne(b => b.Colour);
    }
}
