using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Web.Api.Boards.ListBoards;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/boards/{boardId:int}/[controller]")]
public class ListBoardsController(
    IMapper mapper,
    IListBoardsService listBoardsService,
    IBoardsService boardsService) : ControllerBase
{
    /// <summary>
    /// Get lists by board id.
    /// </summary>
    /// <param name="boardId">Id of the board.</param>
    /// <param name="includeCards">Include cards.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListBoardWithCardDto>>> GetByBoardId(
        int boardId,
        bool? includeCards,
        CancellationToken cancellationToken)
    {
        var typedBoardId = new BoardId(boardId);

        if (!await boardsService.ExistsAsync(typedBoardId, cancellationToken))
        {
            return NotFound($"Board ({boardId}) not found.");
        }
        
        var labels = await listBoardsService.GetByBoardIdAsync(typedBoardId, includeCards, cancellationToken);

        var dtos = mapper.Map<IEnumerable<ListBoardWithCardDto>>(labels);
        
        return Ok(dtos);
    }
}
