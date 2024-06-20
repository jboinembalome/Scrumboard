using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Adherents.Queries.GetAdherents;
using Scrumboard.Application.Adherents.Queries.GetAvatarByIdentityId;

namespace Scrumboard.Web.Api.Adherents;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class AdherentsController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Get all the adherents.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AdherentDto>>> Get()
    {
        var dtos = await mediator.Send(new GetAdherentsQuery());

        return Ok(dtos);
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
        var avatar = await mediator.Send(new GetAvatarByIdentityIdQuery { IdentityId = identityId });
            
        return File(avatar, "image/jpeg");
    }
}
