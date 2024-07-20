using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Users;

namespace Scrumboard.Web.Api.Users;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class UsersController(
    IMapper mapper,
    IUsersService usersService) : ControllerBase
{
    /// <summary>
    /// Get users.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get(CancellationToken cancellationToken)
    {
        var users = await usersService.GetAsync(cancellationToken);
        
        var dtos = mapper.Map<IEnumerable<UserDto>>(users);

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
        var avatar = await usersService.GetAvatarByUserIdAsync(userId, cancellationToken);
            
        return File(avatar, "image/jpeg");
    }
}
