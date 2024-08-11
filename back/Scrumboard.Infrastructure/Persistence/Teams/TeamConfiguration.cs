using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Persistence.Boards;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamConfiguration : CreatedAtEntityTypeConfiguration<Team, TeamId>
{
    protected override void ConfigureDetails(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .HasMany(x => x.Members)
            .WithOne()
            .HasForeignKey(x => x.TeamId);
        
        builder.Property(t => t.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder
            .HasOne<Board>()
            .WithMany()
            .HasForeignKey(x => x.BoardId);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(
                x => (int)x,
                x => (TeamId)x);
        
        builder.Property(x => x.BoardId)
            .HasConversion(
                x => (int)x,
                x => (BoardId)x);
    }
}
