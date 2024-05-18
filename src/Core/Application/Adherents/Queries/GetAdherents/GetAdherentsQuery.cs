using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Adherents.Queries.GetAdherents;

public class GetAdherentsQuery : IRequest<IEnumerable<AdherentDto>>
{
}