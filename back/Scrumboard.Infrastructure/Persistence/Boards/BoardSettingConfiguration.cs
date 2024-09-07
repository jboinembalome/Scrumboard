using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardSettingConfiguration : IEntityTypeConfiguration<BoardSetting>, IModelConfiguration
{
    private const string TableName = "BoardSettings";
    
    public void Configure(EntityTypeBuilder<BoardSetting> builder)
    {
        builder.ToTable(TableName);

        builder
            .HasKey(x => x.Id);
        
        builder
            .OwnsOne(b => b.Colour);
        
        builder.Property(x => x.Id)
            .HasConversion(
                x => (int)x,
                x => x);
    }

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSequence<BoardSetting, BoardSettingId>(increment: 50);
    }
}
