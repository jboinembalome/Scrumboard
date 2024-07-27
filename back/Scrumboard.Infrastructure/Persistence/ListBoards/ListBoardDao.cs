using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Persistence.Cards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.ListBoards;

public sealed class ListBoardDao : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
    public int BoardId { get; set; }
    public ICollection<CardDao> Cards { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
