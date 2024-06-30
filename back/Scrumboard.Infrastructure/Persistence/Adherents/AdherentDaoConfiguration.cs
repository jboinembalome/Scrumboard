using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Adherents;

internal sealed class MemberDaoConfiguration : IEntityTypeConfiguration<AdherentDao>
{
    public void Configure(EntityTypeBuilder<AdherentDao> builder)
    {
        builder.ToTable("Adherents");

        builder.HasKey(x => x.Id);
        
        builder.Property(a => a.Id)
            .HasMaxLength(36);
    }
}
