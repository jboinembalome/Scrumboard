using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.SharedKernel.Types;
using Scrumboard.Web.Api.Teams;

namespace Scrumboard.Web.Api.Boards.Teams;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/boards/{boardId:int}/[controller]")]
public class TeamsController(
    IMapper mapper,
    IBoardsService boardsService,
    ITeamsService teamsService,
    IIdentityService identityService) : ControllerBase
{
    /// <summary>
    /// Get board team.
    /// </summary>
    /// <param name="boardId">Id of the board.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TeamDto>> GetTeamsByBoardId(
        int boardId, 
        CancellationToken cancellationToken)
    {
        var typedBoardId = new BoardId(boardId);

        if (!await boardsService.ExistsAsync(typedBoardId, cancellationToken))
        {
            return NotFound($"Board ({boardId}) not found.");
        }
        
        // TODO: Refactor with Reference
        var team = await teamsService.GetByBoardIdAsync(typedBoardId, cancellationToken);
        
        var teamDto = await GetTeamDtoAsync(team, cancellationToken);
        
        return Ok(teamDto);
    }
    
    private async Task<TeamDto> GetTeamDtoAsync(
        Team team, 
        CancellationToken cancellationToken)
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
