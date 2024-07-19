using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Boards.Labels;

public sealed class LabelCreationModel
{
    public string Name { get; set; }
    public string Colour { get; set; }
    public int BoardId { get; set; }
}
