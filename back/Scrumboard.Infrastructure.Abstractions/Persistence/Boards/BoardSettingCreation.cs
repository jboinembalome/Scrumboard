using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

public sealed class BoardSettingCreation
{
    public Colour Colour { get; set; }
}
