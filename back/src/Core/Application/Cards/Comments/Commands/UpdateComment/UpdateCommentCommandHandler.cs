using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Comments.Specifications;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Comments.Commands.UpdateComment;

internal sealed class UpdateCommentCommandHandler(
    IMapper mapper,
    IAsyncRepository<Comment, int> commentRepository,
    ICurrentUserService currentUserService,
    IIdentityService identityService)
    : IRequestHandler<UpdateCommentCommand, UpdateCommentCommandResponse>
{
    public async Task<UpdateCommentCommandResponse> Handle(
        UpdateCommentCommand request, 
        CancellationToken cancellationToken)
    {
        var updateCommentCommandResponse = new UpdateCommentCommandResponse();

        var specification = new CommentWithAdherentAndCardSpec(request.Id);
        var commentToUpdate = await commentRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (commentToUpdate == null)
            throw new NotFoundException(nameof(Comment), request.Id);

        if (commentToUpdate.Adherent.IdentityId != currentUserService.UserId)
            throw new ForbiddenAccessException();

        mapper.Map(request, commentToUpdate);

        await commentRepository.UpdateAsync(commentToUpdate, cancellationToken);

        var user = await identityService.GetUserAsync(currentUserService.UserId, cancellationToken);
        var commentDto = mapper.Map<CommentDto>(commentToUpdate);

        mapper.Map(user, commentDto.Adherent);
        updateCommentCommandResponse.Comment = commentDto;

        return updateCommentCommandResponse;

        //return Unit.Value;
    }
}
