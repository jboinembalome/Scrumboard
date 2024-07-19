using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

public sealed class LabelEdition
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Colour Colour { get; set; }
    public int BoardId { get; set; }
}
