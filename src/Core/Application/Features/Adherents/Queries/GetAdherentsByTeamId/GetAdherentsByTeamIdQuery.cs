using MediatR;
using Scrumboard.Application.Dto;
using System.Collections.Generic;

namespace Scrumboard.Application.Features.Adherents.Queries.GetAdherentsByTeamId
{
    public class GetAdherentsByTeamIdQuery : IRequest<IEnumerable<AdherentDto>>
    {
        public int TeamId { get; set; }
    }
}
