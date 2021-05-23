using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Infrastructure.Persistence.Configurations
{
    public class BoardSettingConfiguration : IEntityTypeConfiguration<BoardSetting>
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
}
