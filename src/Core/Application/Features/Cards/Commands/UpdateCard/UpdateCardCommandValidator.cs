using FluentValidation;

namespace Scrumboard.Application.Features.Cards.Commands.UpdateCard
{
    public class UpdateCardCommandValidator : AbstractValidator<UpdateCardCommand>
    {
        public UpdateCardCommandValidator()
        {
            RuleFor(p => p.Name)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}
