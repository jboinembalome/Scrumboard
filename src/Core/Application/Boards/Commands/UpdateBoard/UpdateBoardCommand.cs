using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Boards.Commands.UpdateBoard;

public class UpdateBoardCommand : IRequest<UpdateBoardCommandResponse>
{
    public int BoardId { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public TeamDto Team { get; set; }
    public BoardSettingDto BoardSetting { get; set; }
    public IEnumerable<ListBoardDto> ListBoards { get; set; }
}