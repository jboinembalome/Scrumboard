using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Comments.Specifications;
using Scrumboard.Application.Interfaces.Common;
using Scrumboard.Application.Interfaces.Identity;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, UpdateCommentCommandResponse>
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
}
