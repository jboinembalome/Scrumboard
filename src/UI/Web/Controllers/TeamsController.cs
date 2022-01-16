using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Features.Adherents.Queries.GetAdherentsByTeamId;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}
