﻿using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Adherents.GetAdherentsByTeamId;

public class GetAdherentsByTeamIdQuery : IRequest<IEnumerable<AdherentDto>>
{
    public int TeamId { get; set; }
}