using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Teams;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("TeamMembers");

        builder.HasKey(x => new { x.TeamId, x.MemberId });

        builder.Property(x => x.MemberId)
            .HasMaxLength(36);
        
        builder.Property(x => x.TeamId)
            .HasConversion(
                x => (int)x,
                x => (TeamId)x);
        
        builder.Property(x => x.MemberId)
            .HasConversion(
                x => (string)x,
                x => (UserId)x);
    }
}
