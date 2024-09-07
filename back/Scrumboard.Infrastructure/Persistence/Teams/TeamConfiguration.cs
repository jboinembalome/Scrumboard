using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamConfiguration : CreatedAtEntityTypeConfiguration<Team, TeamId>, IModelConfiguration
{
    private const string TableName = "Teams";
    
    protected override void ConfigureEntity(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable(TableName);
        
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
            .HasConversion(
                x => (int)x,
                x => x);
        
        builder.Property(x => x.BoardId)
            .HasConversion(
                x => (int)x,
                x => x);
    }

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSequence<Team, TeamId>(increment: 50);
    }
}
