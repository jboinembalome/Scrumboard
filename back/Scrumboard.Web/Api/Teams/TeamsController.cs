using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Web.Api.Teams;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class TeamsController(
    IMapper mapper,
    ITeamsService teamsService,
    IIdentityService identityService) : ControllerBase
{
    /// <summary>
    /// Get a team by id.
    /// </summary>
    /// <param name="id">Id of the team.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TeamDto>> GetByTeamId(
        int id,
        CancellationToken cancellationToken)
    {
        var team = await teamsService.GetByIdAsync(new TeamId(id), cancellationToken);
        
        var teamDto = mapper.Map<TeamDto>(team);

        return Ok(teamDto);
    }
    
    /// <summary>
    /// Create a team.
    /// </summary>
    /// <param name="teamCreationDto">Team to be created.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(
        TeamCreationDto teamCreationDto,
        CancellationToken cancellationToken)
    {
        var teamCreation = mapper.Map<TeamCreation>(teamCreationDto);

        var team = await teamsService.AddAsync(teamCreation, cancellationToken);

        var teamDto = await GetTeamDtoAsync(team, cancellationToken);
        
        return Ok(teamDto);
    }

    /// <summary>
    /// Update a team.
    /// </summary>
    /// <param name="id">Id of the team.</param>
    /// <param name="teamEditionDto">Team to be updated.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(
        int id, 
        TeamEditionDto teamEditionDto,
        CancellationToken cancellationToken)
    {
        if (id != teamEditionDto.Id)
            return BadRequest();

        var teamEdition = mapper.Map<TeamEdition>(teamEditionDto);

        var team = await teamsService.UpdateAsync(teamEdition, cancellationToken);

        var teamDto = await GetTeamDtoAsync(team, cancellationToken);

        return Ok(teamDto);
    }

    /// <summary>
    /// Delete a team.
    /// </summary>
    /// <param name="id">Id of the team.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(
        int id, 
        CancellationToken cancellationToken)
    {
        await teamsService.DeleteAsync(new TeamId(id), cancellationToken);

        return NoContent();
    }
    
    private async Task<TeamDto> GetTeamDtoAsync(Team team, CancellationToken cancellationToken)
    {
        var memberIds = team.Members
            .Select(x => new UserId(x.MemberId.Value))
            .ToHashSet();
        
        var members = await identityService.GetListAsync(memberIds, cancellationToken);
        
        var teamDto = mapper.Map<TeamDto>(team);
        
        mapper.Map(members, teamDto.Members);
        
        return teamDto;
    }
}
