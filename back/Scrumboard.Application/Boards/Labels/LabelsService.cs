using AutoMapper;
using FluentValidation;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Application.Boards.Labels;

internal sealed class LabelsService(
    IMapper mapper,
    ILabelsQueryRepository labelsQueryRepository,
    ILabelsRepository labelsRepository,
    IValidator<LabelCreation> labelCreationValidator,
    IValidator<LabelEdition> labelEditionValidator) : ILabelsService
{
    public Task<IReadOnlyList<Label>> GetByBoardIdAsync(
        BoardId boardId, 
        CancellationToken cancellationToken = default) 
        => labelsQueryRepository.GetByBoardIdAsync(boardId, cancellationToken);

    public Task<IReadOnlyList<Label>> GetAsync(
        IEnumerable<LabelId> labelIds, 
        CancellationToken cancellationToken = default)
        => labelsQueryRepository.GetAsync(labelIds, cancellationToken);
    
    public Task<Label> GetByIdAsync(
        LabelId id, 
        CancellationToken cancellationToken = default) 
        => labelsQueryRepository.TryGetByIdAsync(id, cancellationToken)
               .OrThrowResourceNotFoundAsync(id);

    public async Task<Label> AddAsync(
        LabelCreation labelCreation, 
        CancellationToken cancellationToken = default)
    {
        await labelCreationValidator.ValidateAndThrowAsync(labelCreation, cancellationToken);

        var label = mapper.Map<Label>(labelCreation);
        
        return await labelsRepository.AddAsync(label, cancellationToken);
    }

    public async Task<Label> UpdateAsync(
        LabelEdition labelEdition, 
        CancellationToken cancellationToken = default)
    {
        await labelEditionValidator.ValidateAndThrowAsync(labelEdition, cancellationToken);
        
        var label = await labelsRepository.TryGetByIdAsync(labelEdition.Id, cancellationToken)
            .OrThrowResourceNotFoundAsync(labelEdition.Id);

        mapper.Map(labelEdition, label);

        labelsRepository.Update(label);
        
        return label; 
    }

    public async Task DeleteAsync(
        LabelId id, 
        CancellationToken cancellationToken = default)
    {
        _ = await labelsRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);
        
        await labelsRepository.DeleteAsync(id, cancellationToken);
    }
}
