using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Cards.Activities.Queries.GetActivitiesByCardId
{
    public class GetActivitiesByCardIdQuery : IRequest<IEnumerable<ActivityDto>>
    {
        public int CardId { get; set; }
    }
}
