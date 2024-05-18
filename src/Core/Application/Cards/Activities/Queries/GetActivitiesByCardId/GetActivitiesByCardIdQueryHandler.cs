using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Activities.Queries.GetActivitiesByCardId;

internal sealed class GetActivitiesByCardIdQueryHandler : IRequestHandler<GetActivitiesByCardIdQuery, IEnumerable<ActivityDto>>
{
    private readonly IAsyncRepository<Activity, int> _activityRepository;
    private readonly IAsyncRepository<Adherent, int> _adherentRepository;
    private readonly IIdentityService _identityService;

    private readonly IMapper _mapper;

    public GetActivitiesByCardIdQueryHandler(
        IMapper mapper, 
        IAsyncRepository<Activity, int> activityRepository, 
        IAsyncRepository<Adherent, int> adherentRepository,
        IIdentityService identityService)
    {
        _mapper = mapper;
        _activityRepository = activityRepository;
        _adherentRepository = adherentRepository;
        _identityService = identityService;
    }

    public async Task<IEnumerable<ActivityDto>> Handle(GetActivitiesByCardIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new AllActivitiesInCardSpec(request.CardId);
        var activities = await _activityRepository.ListAsync(specification, cancellationToken);
        var activityDtos = _mapper.Map<IEnumerable<ActivityDto>>(activities);

        if (activities.Any())
        {
            var users = await _identityService.GetListAsync(activities.Select(a => a.Adherent.IdentityId), cancellationToken);
            var adherentDtos = activityDtos.Select(c => c.Adherent).ToList();

            MapUsers(users, adherentDtos);
        }

        return activityDtos;
    }

    public void MapUsers(IEnumerable<IUser> users, IEnumerable<AdherentDto> adherents)
    {
        foreach (var adherent in adherents)
        {
            var user = users.FirstOrDefault(u => u.Id == adherent.IdentityId);
            if (user == null)
                continue;

            _mapper.Map(user, adherent);
        }
    }
}