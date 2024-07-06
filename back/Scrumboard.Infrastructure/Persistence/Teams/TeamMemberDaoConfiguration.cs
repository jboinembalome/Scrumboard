using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamMemberDaoConfiguration : IEntityTypeConfiguration<TeamMemberDao>
{
    public void Configure(EntityTypeBuilder<TeamMemberDao> builder)
    {
        builder.ToTable("TeamMembers");

        builder.HasKey(x => new { x.TeamId, x.MemberId });

        builder.Property(x => x.MemberId)
            .HasMaxLength(36);
    }
}
