using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Teams.Queries.GetTeamsByBoardId;

namespace Scrumboard.Web.Api.Boards.Teams;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/boards/{boardId}/[controller]")]
public class TeamsController(
    ISender mediator,
    IBoardsService boardsService) : ControllerBase
{
    /// <summary>
    /// Get board team.
    /// </summary>
    /// <param name="boardId">Id of the board.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetTeamsByBoardId(
        int boardId, 
        CancellationToken cancellationToken)
    {
        if (!await boardsService.ExistsAsync(boardId, cancellationToken))
        {
            return NotFound($"Board ({boardId}) not found.");
        }
        
        var dto = await mediator.Send(
            new GetTeamsByBoardIdQuery { BoardId = boardId }, 
            cancellationToken);

        return Ok(dto);
    }
}
