using Scrumboard.Application.Dto;
using Scrumboard.Application.Responses;
using System.Collections.Generic;

namespace Scrumboard.Application.Features.Boards.Commands.UpdateBoard
{
    public class UpdateBoardCommandResponse : BaseResponse
    {
        public UpdateBoardCommandResponse() : base() { }
       
        public IEnumerable<ListBoardDto> ListBoards { get; set; }
    }
}
