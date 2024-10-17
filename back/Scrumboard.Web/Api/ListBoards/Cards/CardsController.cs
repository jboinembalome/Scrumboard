using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Web.Api.Cards;

namespace Scrumboard.Web.Api.ListBoards.Cards;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/list-boards/{listBoardId:int}/[controller]")]
public class CardsController(
    IMapper mapper,
    ICardsService cardsService,
    IListBoardsService listBoardsService) : ControllerBase
{
    /// <summary>
    /// Get cards by listBoard id.
    /// </summary>
    /// <param name="listBoardId">Id of the listBoard.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CardDto>>> GetByBoardId(
        int listBoardId, 
        CancellationToken cancellationToken)
    {
        var typedListBoardId = new ListBoardId(listBoardId);

        if (!await listBoardsService.ExistsAsync(typedListBoardId, cancellationToken))
        {
            return NotFound($"ListBoard ({listBoardId}) not found.");
        }
        
        var labels = await cardsService.GetByListBoardIdAsync(typedListBoardId, cancellationToken);

        var dtos = mapper.Map<IEnumerable<CardDto>>(labels);
        
        return Ok(dtos);
    }
}
