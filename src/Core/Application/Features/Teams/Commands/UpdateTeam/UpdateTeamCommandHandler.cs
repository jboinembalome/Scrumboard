﻿using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Teams.Specifications;
using Scrumboard.Application.Interfaces.Common;
using Scrumboard.Application.Interfaces.Identity;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, UpdateTeamCommandResponse>
    {
        private readonly IAsyncRepository<Team, int> _teamRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public UpdateTeamCommandHandler(IMapper mapper, IAsyncRepository<Team, int> teamRepository, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<UpdateTeamCommandResponse> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var updateTeamCommandResponse = new UpdateTeamCommandResponse();

            var specification = new TeamWithAdherentsSpec(request.Id);
            var teamToUpdate = await _teamRepository.FirstOrDefaultAsync(specification , cancellationToken);

            if (teamToUpdate == null)
                throw new NotFoundException(nameof(Comment), request.Id);

            _mapper.Map(request, teamToUpdate);

            await _teamRepository.UpdateAsync(teamToUpdate, cancellationToken);

            var users = await _identityService.GetListAsync(teamToUpdate.Adherents.Select(a => a.IdentityId), cancellationToken);
            //var adherentDtos = _mapper.Map<IEnumerable<AdherentDto>>(teamToUpdate.Adherents);


            var teamDto = _mapper.Map<TeamDto>(teamToUpdate);
            //teamDto.Adherents = _mapper.Map(users, adherentDtos);

            _mapper.Map(users, teamDto.Adherents);

            updateTeamCommandResponse.Team = teamDto;

            return updateTeamCommandResponse;

            //return Unit.Value;
        }
    }
}
