using System.Collections.Generic;
using Scrumboard.Application.Common.Models;
using Scrumboard.Application.ListBoards.Dtos;

namespace Scrumboard.Application.Boards.Commands.UpdateBoard;

public class UpdateBoardCommandResponse : BaseResponse
{
    public UpdateBoardCommandResponse() : base() { }
       
    public IEnumerable<ListBoardDto> ListBoards { get; set; }
}