using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Abstractions.Boards.Labels;

public sealed class LabelEdition
{
    public LabelId Id { get; set; }
    public string Name { get; set; }
    public Colour Colour { get; set; }
    public BoardId BoardId { get; set; }
}
