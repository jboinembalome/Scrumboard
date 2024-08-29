using FluentValidation;
using Scrumboard.Application.Abstractions.Teams;

namespace Scrumboard.Application.Teams;

internal sealed class TeamCreationValidator : AbstractValidator<TeamCreation>
{
    public TeamCreationValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(255);
    }
}
