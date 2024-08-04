using FluentValidation;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Application.Cards.Comments;

internal sealed class CommentsService(
    ICommentsRepository commentsRepository,
    ICommentsQueryRepository commentsQueryRepository,
    IValidator<CommentCreation> commentCreationValidator,
    IValidator<CommentEdition> commentEditionValidator) : ICommentsService
{
    public Task<Comment> GetByIdAsync(
        CommentId id, 
        CancellationToken cancellationToken = default) 
        => commentsQueryRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);

    public Task<IReadOnlyList<Comment>> GetByCardIdAsync(
        CardId cardId, 
        CancellationToken cancellationToken = default) 
        => commentsQueryRepository.GetByCardIdAsync(cardId, cancellationToken);

    public async Task<Comment> AddAsync(
        CommentCreation commentCreation, 
        CancellationToken cancellationToken = default)
    {
        await commentCreationValidator.ValidateAndThrowAsync(commentCreation, cancellationToken);
        
        return await commentsRepository.AddAsync(commentCreation, cancellationToken);
    }

    public async Task<Comment> UpdateAsync(
        CommentEdition commentEdition, 
        CancellationToken cancellationToken = default)
    {
        await commentsRepository.TryGetByIdAsync(commentEdition.Id, cancellationToken)
            .OrThrowResourceNotFoundAsync(commentEdition.Id);
        
        await commentEditionValidator.ValidateAndThrowAsync(commentEdition, cancellationToken);

        return await commentsRepository.UpdateAsync(commentEdition, cancellationToken);
    }

    public async Task DeleteAsync(
        CommentId id, 
        CancellationToken cancellationToken = default)
    {
        await commentsRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);
        
        await commentsRepository.DeleteAsync(id, cancellationToken);
    }
}
