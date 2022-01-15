using AutoMapper;
using MediatR;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Comments.Specifications;
using Scrumboard.Application.Interfaces.Common;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly IAsyncRepository<Comment, int> _commentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public DeleteCommentCommandHandler(IMapper mapper, IAsyncRepository<Comment, int> commentRepository, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var specification = new CommentWithAdherentSpec(request.CommentId);
            var commentToDelete = await _commentRepository.FirstOrDefaultAsync(specification, cancellationToken);

            if (commentToDelete == null)
                throw new NotFoundException(nameof(Card), request.CommentId);

            if (commentToDelete.Adherent.IdentityId != _currentUserService.UserId)
                throw new ForbiddenAccessException();

            await _commentRepository.DeleteAsync(commentToDelete, cancellationToken);

            return Unit.Value;
        }
    }
}
