using AutoMapper;
using FluentValidation;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Abstractions.Cards.Comments;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Application.Cards.Comments;

internal sealed class CommentsService(
    IMapper mapper,
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
        
        var comment = mapper.Map<Comment>(commentCreation);
        
        return await commentsRepository.AddAsync(comment, cancellationToken);
    }

    public async Task<Comment> UpdateAsync(
        CommentEdition commentEdition, 
        CancellationToken cancellationToken = default)
    {
        await commentEditionValidator.ValidateAndThrowAsync(commentEdition, cancellationToken);
        
        var comment = await commentsRepository.TryGetByIdAsync(commentEdition.Id, cancellationToken)
            .OrThrowResourceNotFoundAsync(commentEdition.Id);

        mapper.Map(commentEdition, comment);
        
        commentsRepository.Update(comment);
        
        return comment;
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
