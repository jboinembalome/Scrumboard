using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Boards.Specifications;
using Scrumboard.Application.Features.Cards.Specifications;
using Scrumboard.Application.Interfaces.Identity;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Cards.Queries.GetCardDetail
{
    public class GetCardDetailQueryHandler : IRequestHandler<GetCardDetailQuery, CardDetailDto>
    {
        private readonly IAsyncRepository<Card, int> _cardRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetCardDetailQueryHandler(IMapper mapper, IAsyncRepository<Card, int> cardRepository, IIdentityService identityService)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
            _identityService = identityService;
        }

        public async Task<CardDetailDto> Handle(GetCardDetailQuery request, CancellationToken cancellationToken)
        {
            var specification = new CardWithAllSpec(request.CardId);
            var card = await _cardRepository.FirstOrDefaultAsync(specification, cancellationToken);

            if (card == null)
                throw new NotFoundException(nameof(Card), request.CardId);

            var cardDto = _mapper.Map<CardDetailDto>(card);

            if (cardDto.Adherents.Any())
            {
                var users = await _identityService.GetListAsync(card.Adherents.Select(a => a.IdentityId), cancellationToken);
                _mapper.Map(users, cardDto.Adherents);
            }

            return cardDto;
        }
    }
}
