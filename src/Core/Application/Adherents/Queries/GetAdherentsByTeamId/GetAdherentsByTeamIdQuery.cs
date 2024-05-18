using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;

namespace Scrumboard.Application.Adherents.Queries.GetAdherentsByTeamId;

public class GetAdherentsByTeamIdQuery : IRequest<IEnumerable<AdherentDto>>
{
    public int TeamId { get; set; }
}