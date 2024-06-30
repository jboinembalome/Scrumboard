using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Cards.Activities.Queries.GetActivitiesByCardId;
using Scrumboard.Application.Cards.Commands.CreateCard;
using Scrumboard.Application.Cards.Commands.DeleteCard;
using Scrumboard.Application.Cards.Commands.UpdateCard;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Cards.Queries.GetCardDetail;

namespace Scrumboard.Web.Api.Cards;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class CardsController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Create a card.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateCardCommandResponse>> Create(
        CreateCardCommand command,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = response.Card.Id }, response);
    }

    /// <summary>
    /// Get a card by id.
    /// </summary>
    /// <param name="id">Id of the card.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CardDetailDto>> Get(
        int id,
        CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(
            new GetCardDetailQuery { CardId = id },
            cancellationToken);

        return Ok(dto);
    }

    /// <summary>
    /// Update a card.
    /// </summary>
    /// <param name="id">Id of the card.</param>
    /// <param name="command">Card to be updated.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(
        int id, 
        UpdateCardCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        var dto = await mediator.Send(command, cancellationToken);

        return Ok(dto);
    }

    /// <summary>
    /// Delete a card.
    /// </summary>
    /// <param name="id">Id of the card.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await mediator.Send(
            new DeleteCardCommand { CardId = id },
            cancellationToken);

        return NoContent();
    }
}
