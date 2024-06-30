using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Cards.Comments.Commands.CreateComment;
using Scrumboard.Application.Cards.Comments.Commands.DeleteComment;
using Scrumboard.Application.Cards.Comments.Commands.UpdateComment;
using Scrumboard.Application.Cards.Comments.Queries.GetCommentsByCardId;

namespace Scrumboard.Web.Api.Cards.Comments;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/cards/{cardId}/[controller]")]
public class CommentsController(
    ISender mediator,
    ICardsService cardsService) : ControllerBase
{
    /// <summary>
    /// Get card comments.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll(
        int cardId, 
        CancellationToken cancellationToken)
    {
        if (!await cardsService.ExistsAsync(cardId, cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }
        
        var dtos = await mediator.Send(
            new GetCommentsByCardIdQuery { CardId = cardId }, 
            cancellationToken);

        return Ok(dtos);
    }
    
    /// <summary>
    /// Create a comment on a card.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="commentCreationModel">Comment to be added.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateCommentCommandResponse>> Create(
        int cardId, 
        CommentCreationModel commentCreationModel,
        CancellationToken cancellationToken)
    {
        if (!await cardsService.ExistsAsync(cardId, cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }
        
        var response = await mediator.Send(
            new CreateCommentCommand { Message = commentCreationModel.Message, CardId = cardId }, 
            cancellationToken);

        return new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };
    }

    /// <summary>
    /// Update a comment on a card.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="id">Id of the comment.</param>
    /// <param name="command">Comment to be updated.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(
        int cardId, 
        int id, 
        UpdateCommentCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();
        
        if (!await cardsService.ExistsAsync(cardId, cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }

        var dto = await mediator.Send(command, cancellationToken);

        return Ok(dto);
    }

    /// <summary>
    /// Delete a comment on a card.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="id">Id of the comment.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(
        int cardId, 
        int id,
        CancellationToken cancellationToken)
    {
        if (!await cardsService.ExistsAsync(cardId, cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }
        
        await mediator.Send(new DeleteCommentCommand { CommentId = id }, cancellationToken);

        return NoContent();
    }
}
