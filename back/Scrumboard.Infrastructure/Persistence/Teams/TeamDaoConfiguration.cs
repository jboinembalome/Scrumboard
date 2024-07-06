using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamDaoConfiguration : IEntityTypeConfiguration<TeamDao>
{
    public void Configure(EntityTypeBuilder<TeamDao> builder)
    {
        builder.ToTable("Teams");
        
        builder.Property(t => t.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder
            .HasMany(x => x.Members)
            .WithOne()
            .HasForeignKey(x => x.TeamId);
    }
}
