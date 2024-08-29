using FluentValidation;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Application.Teams;

internal sealed class TeamEditionValidator : AbstractValidator<TeamEdition>
{
    private readonly ITeamsRepository _teamsRepository;

    public TeamEditionValidator(
        ITeamsRepository teamsRepository)
    {
        _teamsRepository = teamsRepository;

        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(x => x.Id)
            .MustAsync(TeamExistsAsync)
            .WithMessage("{PropertyName} not found.");
    }
    
    private async Task<bool> TeamExistsAsync(TeamId teamId, CancellationToken cancellationToken)
    {
        var team = await _teamsRepository.TryGetByIdAsync(teamId, cancellationToken);

        return team is not null;
    }
}
