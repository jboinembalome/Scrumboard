using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Features.Comments.Commands.CreateComment;
using Scrumboard.Application.Features.Comments.Commands.DeleteComment;
using Scrumboard.Application.Features.Comments.Commands.UpdateComment;
using System.Threading.Tasks;

namespace Scrumboard.Web.Controllers;

[Authorize]
[ApiController]
public class CommentsController : ApiControllerBase
{
    public CommentsController()
    {
    }

    /// <summary>
    /// Create a comment on a card.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateCommentCommandResponse>> Create(CreateCommentCommand command)
    {
        var response = await Mediator.Send(command);

        return new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };
    }

    /// <summary>
    /// Update a comment on a card.
    /// </summary>
    /// <param name="id">Id of the comment.</param>
    /// <param name="command">Comment to be updated.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(int id, UpdateCommentCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var dto = await Mediator.Send(command);

        return Ok(dto);
    }

    /// <summary>
    /// Delete a comment on a card.
    /// </summary>
    /// <param name="id">Id of the comment.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCommentCommand { CommentId = id });

        return NoContent();
    }
}