using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scrumboard.Application.Activities.GetActivitiesByCardId;
using Scrumboard.Application.Cards.CreateCard;
using Scrumboard.Application.Cards.DeleteCard;
using Scrumboard.Application.Cards.GetCardDetail;
using Scrumboard.Application.Cards.UpdateCard;

namespace Scrumboard.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class CardsController : ApiControllerBase
    {
        public CardsController()
        {
        }

        /// <summary>
        /// Create a card.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CreateCardCommandResponse>> Create(CreateCardCommand command)
        {
            var response = await Mediator.Send(command);

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
            var dto = await Mediator.Send(new GetCardDetailQuery { CardId = id });

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

            var dto = await Mediator.Send(command);

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
            await Mediator.Send(new DeleteCardCommand { CardId = id });

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
            var dto = await Mediator.Send(new GetActivitiesByCardIdQuery { CardId = id });

            return Ok(dto);
        }
    }
}
