using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Adherents.Queries.GetAdherentsByTeamId;
using Scrumboard.Application.Teams.Commands.UpdateTeam;

namespace Scrumboard.Web.Api.Teams;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class TeamsController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Get a team by id.
    /// </summary>
    /// <param name="id">Id of the team.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AdherentDto>>> GetByTeamId(
        int id,
        CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(
            new GetAdherentsByTeamIdQuery { TeamId = id },
            cancellationToken);

        return Ok(dto);
    }

    /// <summary>
    /// Update a team.
    /// </summary>
    /// <param name="id">Id of the team.</param>
    /// <param name="command">Team to be updated.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(
        int id, 
        UpdateTeamCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        var dto = await mediator.Send(command, cancellationToken);

        return Ok(dto);
    }

}
