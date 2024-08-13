using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards.Labels;

internal sealed class LabelConfiguration : AuditableEntityTypeConfiguration<Label, LabelId>, IModelConfiguration
{
    private const string TableName = "Labels";
    
    protected override void ConfigureDetails(EntityTypeBuilder<Label> builder)
    {
        builder.ToTable(TableName);
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .OwnsOne(x => x.Colour);

        builder.HasOne<Board>()
            .WithMany()
            .HasForeignKey(x => x.BoardId)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        builder.Property(x => x.Id)
            .HasConversion(
                x => (int)x,
                x => (LabelId)x);
        
        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.BoardId)
            .HasConversion(
                x => (int)x,
                x => (BoardId)x);
    }

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSequence<Label, LabelId>(increment: 1);
    }
}
