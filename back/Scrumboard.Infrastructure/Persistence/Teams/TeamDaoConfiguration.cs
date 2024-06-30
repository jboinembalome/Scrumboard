using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Infrastructure.Persistence.Adherents;

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
            .HasMany(x => x.Adherents)
            .WithMany()
            .UsingEntity(
            "TeamsMembers",
            l => l
                .HasOne(typeof(AdherentDao))
                .WithMany()
                .HasForeignKey("MemberId")
                .HasPrincipalKey(nameof(AdherentDao.Id)),
            r => r
                .HasOne(typeof(TeamDao))
                .WithMany()
                .HasForeignKey("TeamId")
                .HasPrincipalKey(nameof(TeamDao.Id)),
            j => j.HasKey("TeamId", "MemberId"));
    }
}
