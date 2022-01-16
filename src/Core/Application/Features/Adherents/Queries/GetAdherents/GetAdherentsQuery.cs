using MediatR;
using Scrumboard.Application.Dto;
using System.Collections.Generic;

namespace Scrumboard.Application.Features.Adherents.Queries.GetAdherents
{
    public class GetAdherentsQuery : IRequest<IEnumerable<AdherentDto>>
    {
    }
}
