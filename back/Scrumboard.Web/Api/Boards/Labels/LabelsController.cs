using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Boards.Labels.Commands.DeleteLabel;

namespace Scrumboard.Web.Api.Boards.Labels;

//[Authorize]
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
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteLabelCommand { LabelId = id });

        return NoContent();
    }
}
