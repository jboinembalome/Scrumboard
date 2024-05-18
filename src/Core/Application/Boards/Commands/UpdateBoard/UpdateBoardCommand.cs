using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.ListBoards.Dtos;
using Scrumboard.Application.Teams.Dtos;

namespace Scrumboard.Application.Boards.Commands.UpdateBoard;

public sealed class UpdateBoardCommand : IRequest<UpdateBoardCommandResponse>
{
    public int BoardId { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public TeamDto Team { get; set; }
    public BoardSettingDto BoardSetting { get; set; }
    public IEnumerable<ListBoardDto> ListBoards { get; set; }
}