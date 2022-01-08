using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Features.Adherents.Queries.GetAdherentsByTeamId;
using Scrumboard.Application.Features.Adherents.Queries.GetAvatarByIdentityId;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrumboard.Web.Controllers
{
    //[Authorize] // TODO: Add Angular pipe to use Authorize (to add the token when we use <img> with src)
    [ApiController]
    public class AdherentsController : ApiControllerBase
    {
        public AdherentsController()
        {
        }

        /// <summary>
        /// Get avatar by identity Id of the adherent.
        /// </summary>
        /// <param name="identityId">Identity Id of the adherent.</param>
        /// <returns></returns>
        [HttpGet("avatar/{identityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAvatar(string identityId)
        {
            var avatar = await Mediator.Send(new GetAvatarByIdentityIdQuery { IdentityId = identityId });
            
            return File(avatar, "image/jpeg");
        }

        /// <summary>
        /// Get adherents by team id.
        /// </summary>
        /// <param name="teamId">Id of the team.</param>
        /// <returns></returns>
        [HttpGet("teams/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AdherentDto>>> GetByTeamId(int teamId)
        {
            var dto = await Mediator.Send(new GetAdherentsByTeamIdQuery { TeamId = teamId });

            return Ok(dto);
        }
    }
}
