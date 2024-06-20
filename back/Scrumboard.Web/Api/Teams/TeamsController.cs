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
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AdherentDto>>> GetByTeamId(int id)
    {
        var dto = await mediator.Send(new GetAdherentsByTeamIdQuery { TeamId = id });

        return Ok(dto);
    }

    /// <summary>
    /// Update a team.
    /// </summary>
    /// <param name="id">Id of the team.</param>
    /// <param name="command">Team to be updated.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(int id, UpdateTeamCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var dto = await mediator.Send(command);

        return Ok(dto);
    }

}
