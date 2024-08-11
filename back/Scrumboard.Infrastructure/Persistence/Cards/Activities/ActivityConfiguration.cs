using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Infrastructure.Persistence.Cards.Activities;

internal sealed class ActivityConfiguration : AuditableEntityTypeConfiguration<Activity, ActivityId>
{
    protected override void ConfigureDetails(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activities");
        
        builder
            .HasKey(x => x.Id);
        
        builder.Property(a => a.ActivityType)
            .IsRequired();

        builder.Property(a => a.OldValue)
            .HasMaxLength(1000);

        builder.Property(a => a.NewValue)
            .HasMaxLength(1000);
        
        builder
            .OwnsOne(b => b.ActivityField);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(
                x => (int)x,
                x => (ActivityId)x);
        
        builder.Property(x => x.CardId)
            .HasConversion(
                x => (int)x,
                x => (CardId)x);
    }
}
