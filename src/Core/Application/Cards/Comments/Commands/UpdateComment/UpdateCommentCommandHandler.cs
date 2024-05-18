using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Comments.Commands.UpdateComment;

internal sealed class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, UpdateCommentCommandResponse>
{
    private readonly IAsyncRepository<Comment, int> _commentRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public UpdateCommentCommandHandler(IMapper mapper, IAsyncRepository<Comment, int> commentRepository, ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _mapper = mapper;
        _commentRepository = commentRepository;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task<UpdateCommentCommandResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var updateCommentCommandResponse = new UpdateCommentCommandResponse();

        var specification = new CommentWithAdherentAndCardSpec(request.Id);
        var commentToUpdate = await _commentRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (commentToUpdate == null)
            throw new NotFoundException(nameof(Comment), request.Id);

        if (commentToUpdate.Adherent.IdentityId != _currentUserService.UserId)
            throw new ForbiddenAccessException();

        _mapper.Map(request, commentToUpdate);

        await _commentRepository.UpdateAsync(commentToUpdate, cancellationToken);

        var user = await _identityService.GetUserAsync(_currentUserService.UserId, cancellationToken);
        var commentDto = _mapper.Map<CommentDto>(commentToUpdate);

        _mapper.Map(user, commentDto.Adherent);
        updateCommentCommandResponse.Comment = commentDto;

        return updateCommentCommandResponse;

        //return Unit.Value;
    }
}