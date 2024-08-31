using Scrumboard.Domain.Common;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Boards.Labels;

public sealed class Label : AuditableEntityBase<LabelId>
{
    public Label() { }
    
    public Label(string name, Colour colour, BoardId boardId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Name = name;
        Colour = colour;
        BoardId = boardId;
    }

    public string Name { get; private set; }
    public Colour Colour { get; private set; }
    public BoardId BoardId { get; private set; }
}
