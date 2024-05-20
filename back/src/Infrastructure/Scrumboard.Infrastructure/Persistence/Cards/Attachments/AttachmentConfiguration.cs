using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards.Attachments;

namespace Scrumboard.Infrastructure.Persistence.Cards.Attachments;

internal sealed class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
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