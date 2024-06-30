using AutoMapper;
using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Infrastructure.Persistence.Cards.Activities;

internal sealed class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        // Write
        CreateMap<Activity, ActivityDao>();

        // Read
        CreateMap<ActivityDao, Activity>();
    }
}
