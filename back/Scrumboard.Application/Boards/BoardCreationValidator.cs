using FluentValidation;
using Scrumboard.Application.Abstractions.Boards;

namespace Scrumboard.Application.Boards;

internal sealed class BoardCreationValidator : AbstractValidator<BoardCreation>
{
    public BoardCreationValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
