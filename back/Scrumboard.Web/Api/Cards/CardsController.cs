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
    public async Task<ActionResult<CreateCardCommandResponse>> Create(CreateCardCommand command)
    {
        var response = await mediator.Send(command);

        return CreatedAtAction(nameof(Get), new { id = response.Card.Id }, response);
    }

    /// <summary>
    /// Get a card by id.
    /// </summary>
    /// <param name="id">Id of the card.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CardDetailDto>> Get(int id)
    {
        var dto = await mediator.Send(new GetCardDetailQuery { CardId = id });

        return Ok(dto);
    }

    /// <summary>
    /// Update a card.
    /// </summary>
    /// <param name="id">Id of the card.</param>
    /// <param name="command">Card to be updated.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(int id, UpdateCardCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var dto = await mediator.Send(command);

        return Ok(dto);
    }

    /// <summary>
    /// Delete a card.
    /// </summary>
    /// <param name="id">Id of the card.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteCardCommand { CardId = id });

        return NoContent();
    }

    /// <summary>
    /// Get activities by card id.
    /// </summary>
    /// <param name="id">Id of the card.</param>
    /// <returns></returns>
    [HttpGet("{id}/activities")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ActivityDto>>> GetActivitiesByCardId(int id)
    {
        var dto = await mediator.Send(new GetActivitiesByCardIdQuery { CardId = id });

        return Ok(dto);
    }
}
