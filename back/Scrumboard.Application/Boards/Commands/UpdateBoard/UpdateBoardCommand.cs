using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.ListBoards.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Boards.Commands.UpdateBoard;

public sealed class UpdateBoardCommand : IRequest<UpdateBoardCommandResponse>
{
    public int BoardId { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public BoardSettingDto BoardSetting { get; set; }
    public IEnumerable<ListBoardDto> ListBoards { get; set; }
}
