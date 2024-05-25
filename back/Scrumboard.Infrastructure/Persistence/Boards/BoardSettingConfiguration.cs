using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardSettingConfiguration : IEntityTypeConfiguration<BoardSetting>
{
    public void Configure(EntityTypeBuilder<BoardSetting> builder)
    {
        builder.Property(b => b.CardCoverImage)
            .IsRequired();

        builder.Property(b => b.Subscribed)
            .IsRequired();

        builder
            .OwnsOne(b => b.Colour);
    }
}