using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Attachments;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Infrastructure.Persistence.Configurations
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.Property(a => a.Name)
                .IsRequired();

            builder.Property(a => a.Url)
                .IsRequired();

            builder.Property(a => a.AttachmentType)
               .IsRequired();
        }
    }
}
