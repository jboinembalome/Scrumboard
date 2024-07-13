﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Application.Cards.Comments.Commands.UpdateComment;

internal sealed class UpdateCommentCommandHandler(
    IMapper mapper,
    ICommentsRepository commentsRepository,
    IValidator<UpdateCommentCommand> updateCommentCommentValidator,
    ICurrentUserService currentUserService,
    IIdentityService identityService)
    : IRequestHandler<UpdateCommentCommand, UpdateCommentCommandResponse>
{
    public async Task<UpdateCommentCommandResponse> Handle(
        UpdateCommentCommand request, 
        CancellationToken cancellationToken)
    {
        await updateCommentCommentValidator.ValidateAndThrowAsync(request, cancellationToken);
        
        var updateCommentCommandResponse = new UpdateCommentCommandResponse();
        
        var commentToUpdate = await commentsRepository.TryGetByIdAsync(request.Id, cancellationToken);

        if (commentToUpdate is null)
            throw new NotFoundException(nameof(Comment), request.Id);

        // TODO: Add Policy for that?
        if (commentToUpdate.CreatedBy != currentUserService.UserId)
            throw new ForbiddenAccessException();

        mapper.Map(request, commentToUpdate);

        await commentsRepository.UpdateAsync(commentToUpdate, cancellationToken);

        var user = await identityService.GetUserAsync(currentUserService.UserId, cancellationToken);
        
        var commentDto = mapper.Map<CommentDto>(commentToUpdate);

        mapper.Map(user, commentDto.User);
        
        updateCommentCommandResponse.Comment = commentDto;

        return updateCommentCommandResponse;
    }
}
