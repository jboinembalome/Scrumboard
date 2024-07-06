using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Users.Dtos;
using Scrumboard.Application.Users.Queries.GetAvatarByUserId;
using Scrumboard.Application.Users.Queries.GetUsers;

namespace Scrumboard.Web.Api.Users;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class UsersController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Get users.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get(CancellationToken cancellationToken)
    {
        var dtos = await mediator.Send(new GetUsersQuery(), cancellationToken);

        return Ok(dtos);
    }

    /// <summary>
    /// Get avatar by user id.
    /// </summary>
    /// <param name="userId">User id.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("avatar/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAvatar(string userId, CancellationToken cancellationToken)
    {
        var avatar = await mediator.Send(
            new GetAvatarByUserIdQuery { UserId = userId }, 
            cancellationToken);
            
        return File(avatar, "image/jpeg");
    }
}
