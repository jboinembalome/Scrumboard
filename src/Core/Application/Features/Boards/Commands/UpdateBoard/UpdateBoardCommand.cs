using MediatR;
using Scrumboard.Application.Dto;
using System.Collections.Generic;

namespace Scrumboard.Application.Features.Boards.Commands.UpdateBoard
{
    public class UpdateBoardCommand : IRequest
    {
        public int BoardId { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public TeamDto Team { get; set; }
        public BoardSettingDto BoardSetting { get; set; }
        public IEnumerable<ListBoardDto> ListBoards { get; set; }
    }
}
