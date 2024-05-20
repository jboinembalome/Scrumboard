using MediatR;
using Scrumboard.Application.Adherents.Dtos;

namespace Scrumboard.Application.Adherents.Queries.GetAdherentsByTeamId;

public sealed class GetAdherentsByTeamIdQuery : IRequest<IEnumerable<AdherentDto>>
{
    public int TeamId { get; set; }
}