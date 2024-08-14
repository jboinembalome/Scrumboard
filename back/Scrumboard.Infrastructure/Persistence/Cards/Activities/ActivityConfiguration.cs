using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Infrastructure.Persistence.Cards.Activities;

internal sealed class ActivityConfiguration : CreatedAtEntityTypeConfiguration<Activity, ActivityId>, IModelConfiguration
{
    private const string TableName = "Activities";
    
    protected override void ConfigureEntity(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable(TableName);
        
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
            .HasConversion(
                x => (int)x,
                x => (ActivityId)x);
        
        builder.Property(x => x.CardId)
            .HasConversion(
                x => (int)x,
                x => (CardId)x);
    }

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSequence<Activity, ActivityId>(increment: 50);
    }
}
