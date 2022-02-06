using MediatR;
using Scrumboard.Application.Dto;
using System.Collections.Generic;

namespace Scrumboard.Application.Features.Activities.Queries.GetActivitiesByCardId
{
    public class GetActivitiesByCardIdQuery : IRequest<IEnumerable<ActivityDto>>
    {
        public int CardId { get; set; }
    }
}
