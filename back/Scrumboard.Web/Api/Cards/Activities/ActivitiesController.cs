using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Web.Api.Users;

namespace Scrumboard.Web.Api.Cards.Activities;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/cards/{cardId:int}/[controller]")]
public class ActivitiesController(
    IMapper mapper,
    IActivitiesService activitiesService,
    ICardsService cardsService,
    IIdentityService identityService) : ControllerBase
{
    /// <summary>
    /// Get card activities.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ActivityDto>>> GetActivitiesByCardId(
        int cardId, 
        CancellationToken cancellationToken)
    {
        var typedCardId = new CardId(cardId);

        if (!await cardsService.ExistsAsync(typedCardId, cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }

        var activities = await activitiesService.GetByCardIdAsync(typedCardId, cancellationToken);
        
        if (activities.Count == 0)
        {
            return Ok(Array.Empty<ActivityDto>());
        }
        
        var activityDtos = await GetActivityDtosAsync(activities, cancellationToken);

        return Ok(activityDtos);
    }

    private async Task<IReadOnlyCollection<ActivityDto>> GetActivityDtosAsync(
        IReadOnlyCollection<Activity> activities,
        CancellationToken cancellationToken)
    {
        var activityDtos = mapper.Map<IEnumerable<ActivityDto>>(activities).ToList();
        
        var users = await identityService
            .GetListAsync(activities
                .Select(a => a.CreatedBy), cancellationToken);
        
        var userDtos = activityDtos.Select(c => c.User).ToList();

        MapUsers(users, userDtos);

        return activityDtos;
    }
    
    private void MapUsers(IReadOnlyList<IUser> users, IEnumerable<UserDto> userDtos)
    {
        foreach (var userDto in userDtos)
        {
            var user = users.FirstOrDefault(u => u.Id == userDto.Id);

            if (user is null)
            {
                continue;
            }

            mapper.Map(user, userDto);
        }
    }
}
