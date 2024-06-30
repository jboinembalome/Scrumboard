using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Cards.Labels.Commands.DeleteLabel;

namespace Scrumboard.Web.Api.Cards.Labels;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class LabelsController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Delete a label on a board.
    /// </summary>
    /// <remarks>Delete the label from all cards in the board.</remarks>
    /// <param name="id">Id of the label.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(
            new DeleteLabelCommand { LabelId = id },
            cancellationToken);

        return NoContent();
    }
}
