﻿using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Adherents.Queries.GetAdherents;
using Scrumboard.Application.Adherents.Queries.GetAvatarByIdentityId;

namespace Scrumboard.Web.Controllers;

//[Authorize] // TODO: Add Angular pipe to use Authorize (to add the token when we use <img> with src)
[ApiController]
public class AdherentsController : ApiControllerBase
{
    public AdherentsController()
    {
    }

    /// <summary>
    /// Get all the adherents.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AdherentDto>>> Get()
    {
        var dtos = await Mediator.Send(new GetAdherentsQuery());

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
        var avatar = await Mediator.Send(new GetAvatarByIdentityIdQuery { IdentityId = identityId });
            
        return File(avatar, "image/jpeg");
    }
}