using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Cards.Activities.Queries.GetActivitiesByCardId;

namespace Scrumboard.Web.Api.Cards.Activities;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/cards/{cardId}/[controller]")]
public class ActivitiesController(
    ISender mediator,
    ICardsService cardsService) : ControllerBase
{
    /// <summary>
    /// Get card activities.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetActivitiesByCardId(
        int cardId, 
        CancellationToken cancellationToken)
    {
        if (!await cardsService.ExistsAsync(cardId, cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }
        
        var dtos = await mediator.Send(
            new GetActivitiesByCardIdQuery { CardId = cardId }, 
            cancellationToken);

        return Ok(dtos);
    }
}
