using FluentValidation;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Application.Boards.Labels;

internal sealed class LabelsService(
    ILabelsQueryRepository labelsQueryRepository,
    ILabelsRepository labelsRepository,
    IValidator<LabelCreation> labelCreationValidator,
    IValidator<LabelEdition> labelEditionValidator) : ILabelsService
{
    public Task<IReadOnlyList<Label>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default) 
        => labelsQueryRepository.GetByBoardIdAsync(boardId, cancellationToken);

    public async Task<Label> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
        => await labelsQueryRepository.TryGetByIdAsync(id, cancellationToken) 
           ?? throw new NotFoundException(nameof(Label), id);

    public async Task<Label> AddAsync(LabelCreation labelCreation, CancellationToken cancellationToken = default)
    {
        await labelCreationValidator.ValidateAndThrowAsync(labelCreation, cancellationToken);
        
        return await labelsRepository.AddAsync(labelCreation, cancellationToken);
    }

    public async Task<Label> UpdateAsync(LabelEdition labelEdition, CancellationToken cancellationToken = default)
    {
        _ = await labelsRepository.TryGetByIdAsync(labelEdition.Id, cancellationToken) 
            ?? throw new NotFoundException(nameof(Label), labelEdition.Id);
        
        await labelEditionValidator.ValidateAndThrowAsync(labelEdition, cancellationToken);

        return await labelsRepository.UpdateAsync(labelEdition, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _ = await labelsRepository.TryGetByIdAsync(id, cancellationToken) 
            ?? throw new NotFoundException(nameof(Label), id);
        
        await labelsRepository.DeleteAsync(id, cancellationToken);
    }
}
