using FluentValidation;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Application.Cards.Comments;

internal sealed class CommentEditionValidator : AbstractValidator<CommentEdition>
{
    public CommentEditionValidator()
    {
        RuleFor(p => p.Message)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");
    }
}
