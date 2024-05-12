using System.Collections.Generic;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Responses;

namespace Scrumboard.Application.Boards.Commands.UpdateBoard;

public class UpdateBoardCommandResponse : BaseResponse
{
    public UpdateBoardCommandResponse() : base() { }
       
    public IEnumerable<ListBoardDto> ListBoards { get; set; }
}