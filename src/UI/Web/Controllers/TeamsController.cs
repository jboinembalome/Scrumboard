using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scrumboard.Application.Adherents.GetAdherentsByTeamId;
using Scrumboard.Application.Teams.UpdateTeam;

namespace Scrumboard.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class TeamsController : ApiControllerBase
    {
        public TeamsController()
        {
        }

        /// <summary>
        /// Get a team by id.
        /// </summary>
        /// <param name="id">Id of the team.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AdherentDto>>> GetByTeamId(int id)
        {
            var dto = await Mediator.Send(new GetAdherentsByTeamIdQuery { TeamId = id });

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

            var dto = await Mediator.Send(command);

            return Ok(dto);
        }

    }
}
