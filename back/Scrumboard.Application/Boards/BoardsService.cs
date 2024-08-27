using AutoMapper;
using FluentValidation;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Application.Boards;

internal sealed class BoardsService(
    IMapper mapper,
    IBoardsRepository boardsRepository,
    IBoardsQueryRepository boardsQueryRepository,
    IValidator<BoardCreation> boardCreationValidator,
    IValidator<BoardEdition> boardEditionValidator,
    ICurrentUserService currentUserService) : IBoardsService
{
    public async Task<bool> ExistsAsync(
        BoardId id, 
        CancellationToken cancellationToken = default)
    {
        var board = await boardsRepository.TryGetByIdAsync(id, cancellationToken);

        return board is not null;
    }

    public async Task<Board> GetByIdAsync(
        BoardId id, 
        CancellationToken cancellationToken = default) 
        => await boardsQueryRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);

    public Task<IReadOnlyList<Board>> GetAsync(
        CancellationToken cancellationToken = default)
        => boardsQueryRepository.GetByOwnerIdAsync(currentUserService.UserId, cancellationToken);

    public async Task<Board> AddAsync(
        BoardCreation boardCreation, 
        CancellationToken cancellationToken = default)
    {
        boardCreation.OwnerId = currentUserService.UserId;
        
        await boardCreationValidator.ValidateAndThrowAsync(boardCreation, cancellationToken);

        var board = mapper.Map<Board>(boardCreation);
        
        await boardsRepository.AddAsync(board, cancellationToken);
        
        board.MarkAsCreated();
        
        return board;
    }

    public async Task<Board> UpdateAsync(
        BoardEdition boardEdition, 
        CancellationToken cancellationToken = default)
    {
        await boardEditionValidator.ValidateAndThrowAsync(boardEdition, cancellationToken);
        
        var board = await boardsRepository.TryGetByIdAsync(boardEdition.Id, cancellationToken)
            .OrThrowResourceNotFoundAsync(boardEdition.Id);

        board.Update(
            name: boardEdition.Name,
            isPinned: boardEdition.IsPinned,
            boardSettingColour: boardEdition.BoardSetting.Colour);

        boardsRepository.Update(board);
        
        return board;
    }

    public async Task DeleteAsync(
        BoardId id, 
        CancellationToken cancellationToken = default)
    {
        await boardsRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);
        
        await boardsRepository.DeleteAsync(id, cancellationToken);
    }
}
