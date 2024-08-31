using FluentValidation;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Application.Boards.Labels;

internal sealed class LabelEditionValidator : AbstractValidator<LabelEdition>
{
    private readonly ILabelsRepository _labelsRepository;
    private readonly IBoardsRepository _boardsRepository;

    public LabelEditionValidator(
        ILabelsRepository labelsRepository,
        IBoardsRepository boardsRepository)
    {
        _labelsRepository = labelsRepository;
        _boardsRepository = boardsRepository;

        RuleFor(p => p.Name)
            .NotEmpty()
                .WithMessage("{PropertyName} is required.")
            .MaximumLength(255)
                .WithMessage("{PropertyName} must not exceed 255 characters.");

        RuleFor(x => x.Id)
            .MustAsync(LabelExistsAsync)
            .WithMessage("{PropertyName} not found.");
            
        RuleFor(x => x.BoardId)
            .MustAsync(BoardExistsAsync)
                .WithMessage("{PropertyName} not found.");
    }
    
    private async Task<bool> LabelExistsAsync(LabelId id, CancellationToken cancellationToken)
    {
        var label = await _labelsRepository.TryGetByIdAsync(id, cancellationToken);

        return label is not null;
    }
    
    private async Task<bool> BoardExistsAsync(BoardId boardId, CancellationToken cancellationToken)
    {
        var board = await _boardsRepository.TryGetByIdAsync(boardId, cancellationToken);

        return board is not null;
    }
}
