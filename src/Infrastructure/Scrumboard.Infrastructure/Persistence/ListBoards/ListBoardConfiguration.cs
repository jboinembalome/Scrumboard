using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Persistence.ListBoards;

public class ListBoardConfiguration : IEntityTypeConfiguration<ListBoard>
{
    public void Configure(EntityTypeBuilder<ListBoard> builder)
    {
        builder.Property(l => l.Name)
            .IsRequired();
    }
}