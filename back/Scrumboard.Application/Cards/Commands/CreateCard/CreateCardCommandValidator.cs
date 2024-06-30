using FluentValidation;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards.ListBoards;

namespace Scrumboard.Application.Cards.Commands.CreateCard;

internal sealed class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
{
    private readonly IListBoardsRepository _listBoardsRepository;

    public CreateCardCommandValidator(IListBoardsRepository listBoardsRepository)
    {
        _listBoardsRepository = listBoardsRepository;
        
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(255).WithMessage("{PropertyName} must not exceed 50 characters.");
        
        RuleFor(x => x.ListBoardId)
            .NotEmpty()
            .MustAsync(ListBoardExists)
            .WithMessage("ListBoard ({PropertyValue}) not found.");
    }
    
    private async Task<bool> ListBoardExists(int listBoardId, CancellationToken cancellationToken)
    {
        var listBoard = await _listBoardsRepository.TryGetByIdAsync(listBoardId, cancellationToken);
        
        return listBoard is not null;
    }
}
