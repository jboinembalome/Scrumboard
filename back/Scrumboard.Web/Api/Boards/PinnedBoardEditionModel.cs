namespace Scrumboard.Web.Api.Boards;

public sealed class PinnedBoardEditionModel
{
    public int BoardId { get; set; }
    public bool IsPinned { get; set; }
}
