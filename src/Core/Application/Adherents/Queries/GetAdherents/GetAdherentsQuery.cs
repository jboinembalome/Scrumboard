using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;

namespace Scrumboard.Application.Adherents.Queries.GetAdherents;

public sealed class GetAdherentsQuery : IRequest<IEnumerable<AdherentDto>>
{
}