using FluentValidation;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.Boards;

internal sealed class BoardEditionValidator : AbstractValidator<BoardEdition>
{
    public BoardEditionValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
    }
}
