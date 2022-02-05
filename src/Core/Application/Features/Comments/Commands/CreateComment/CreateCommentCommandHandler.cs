using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Adherents.Specifications;
using Scrumboard.Application.Features.Cards.Specifications;
using Scrumboard.Application.Interfaces.Common;
using Scrumboard.Application.Interfaces.Identity;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using Scrumboard.Domain.Enums;
using Scrumboard.Domain.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CreateCommentCommandResponse>
    {
        private readonly IAsyncRepository<Comment, int> _commentRepository;
        private readonly IAsyncRepository<Card, int> _cardRepository;
        private readonly IAsyncRepository<Adherent, int> _adherentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public CreateCommentCommandHandler(IMapper mapper, IAsyncRepository<Comment, int> commentRepository, IAsyncRepository<Card, int> cardRepository, IAsyncRepository<Adherent, int> adherentRepository, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
            _cardRepository = cardRepository;
            _adherentRepository = adherentRepository;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var createCommentCommandResponse = new CreateCommentCommandResponse();

            var cardSpecification = new CardWithActivitiesSpec(request.CardId);
            var card = await _cardRepository.FirstOrDefaultAsync(cardSpecification, cancellationToken);

            if (card == null)
                throw new NotFoundException(nameof(Card), request.CardId);

            var specification = new AdherentByUserIdSpec(_currentUserService.UserId);
            var adherent = await _adherentRepository.FirstAsync(specification, cancellationToken);

            var comment = _mapper.Map<Comment>(request);
            comment.Adherent = adherent;
            comment.Card = card;

            var activity = new Activity(ActivityType.Added, ActivityField.Comment, string.Empty, request.Message);
            activity.Adherent = adherent;
            comment.Card.Activities.Add(activity);

            comment = await _commentRepository.AddAsync(comment, cancellationToken);

            var user = await _identityService.GetUserAsync(_currentUserService.UserId, cancellationToken);
            var commentDto = _mapper.Map<CommentDto>(comment);

            _mapper.Map(user, commentDto.Adherent);
            createCommentCommandResponse.Comment = commentDto;

            return createCommentCommandResponse;
        }
    }
}
