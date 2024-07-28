using FluentValidation;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Application.Cards.Comments;

internal sealed class CommentsService(
    ICommentsRepository commentsRepository,
    ICommentsQueryRepository commentsQueryRepository,
    IValidator<CommentCreation> commentCreationValidator,
    IValidator<CommentEdition> commentEditionValidator) : ICommentsService
{
    public async Task<Comment> GetByIdAsync(CommentId id, CancellationToken cancellationToken = default) 
        => await commentsQueryRepository.TryGetByIdAsync(id, cancellationToken) 
           ?? throw new NotFoundException(nameof(Comment), id);

    public Task<IReadOnlyList<Comment>> GetByCardIdAsync(CardId cardId, CancellationToken cancellationToken = default) 
        => commentsQueryRepository.GetByCardIdAsync(cardId, cancellationToken);

    public async Task<Comment> AddAsync(CommentCreation commentCreation, CancellationToken cancellationToken = default)
    {
        await commentCreationValidator.ValidateAndThrowAsync(commentCreation, cancellationToken);
        
        return await commentsRepository.AddAsync(commentCreation, cancellationToken);
    }

    public async Task<Comment> UpdateAsync(CommentEdition commentEdition, CancellationToken cancellationToken = default)
    {
        _ = await commentsRepository.TryGetByIdAsync(commentEdition.Id, cancellationToken) 
            ?? throw new NotFoundException(nameof(Comment), commentEdition.Id);
        
        await commentEditionValidator.ValidateAndThrowAsync(commentEdition, cancellationToken);

        return await commentsRepository.UpdateAsync(commentEdition, cancellationToken);
    }

    public async Task DeleteAsync(CommentId id, CancellationToken cancellationToken = default)
    {
        _ = await commentsRepository.TryGetByIdAsync(id, cancellationToken) 
            ?? throw new NotFoundException(nameof(Comment), id);
        
        await commentsRepository.DeleteAsync(id, cancellationToken);
    }
}
