using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Abstractions.Boards;

public sealed class BoardSettingEdition
{
    public Colour Colour { get; set; }
}
