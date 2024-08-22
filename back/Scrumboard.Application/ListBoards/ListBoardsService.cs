using AutoMapper;
using FluentValidation;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Application.ListBoards;

internal sealed class ListBoardsService(
    IMapper mapper,
    IListBoardsQueryRepository listBoardsQueryRepository,
    IListBoardsRepository listBoardsRepository,
    IValidator<ListBoardCreation> listBoardCreationValidator,
    IValidator<ListBoardEdition> listBoardEditionValidator) : IListBoardsService
{
    public async Task<bool> ExistsAsync(
        ListBoardId id, 
        CancellationToken cancellationToken = default)
    {
        var listBoard = await listBoardsRepository.TryGetByIdAsync(id, cancellationToken);

        return listBoard is not null;
    }
    
    public Task<IReadOnlyList<ListBoard>> GetByBoardIdAsync(
        BoardId boardId, 
        bool? includeCards, 
        CancellationToken cancellationToken = default)
        => listBoardsQueryRepository.GetByBoardIdAsync(boardId, includeCards, cancellationToken);

    public Task<ListBoard> GetByIdAsync(
        ListBoardId id, 
        CancellationToken cancellationToken = default)
        => listBoardsQueryRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);

    public async Task<ListBoard> AddAsync(
        ListBoardCreation listBoardCreation, 
        CancellationToken cancellationToken = default)
    {
        await listBoardCreationValidator.ValidateAndThrowAsync(listBoardCreation, cancellationToken);

        var listBoard = mapper.Map<ListBoard>(listBoardCreation);
        
        return await listBoardsRepository.AddAsync(listBoard, cancellationToken);
    }

    public async Task<ListBoard> UpdateAsync(
        ListBoardEdition listBoardEdition, 
        CancellationToken cancellationToken = default)
    {
        await listBoardEditionValidator.ValidateAndThrowAsync(listBoardEdition, cancellationToken);
        
        var listBoard = await listBoardsRepository.TryGetByIdAsync(listBoardEdition.Id, cancellationToken)
            .OrThrowResourceNotFoundAsync(listBoardEdition.Id);

        mapper.Map(listBoardEdition, listBoard);

        listBoardsRepository.Update(listBoard);
            
        return listBoard;
    }

    public async Task DeleteAsync(
        ListBoardId id, 
        CancellationToken cancellationToken = default)
    {
        await listBoardsRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);
        
        await listBoardsRepository.DeleteAsync(id, cancellationToken);
    }
}
