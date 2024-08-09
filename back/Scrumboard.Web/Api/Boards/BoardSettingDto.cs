
using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Boards;

public sealed class BoardSettingDto
{
    public int Id { get; set; }
    public Colour Colour { get; set; }
}
