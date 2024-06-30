using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Cards.Activities;

internal sealed class ActivityDaoConfiguration : IEntityTypeConfiguration<ActivityDao>
{
    public void Configure(EntityTypeBuilder<ActivityDao> builder)
    {
        builder.ToTable("Activities");
        
        builder.Property(a => a.ActivityType)
            .IsRequired();

        builder.Property(a => a.OldValue)
            .HasMaxLength(1000);

        builder.Property(a => a.NewValue)
            .HasMaxLength(1000);
        
        builder
            .OwnsOne(b => b.ActivityField);
    }
}
