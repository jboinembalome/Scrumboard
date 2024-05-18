using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Queries.GetCardDetail;

internal sealed class GetCardDetailQueryHandler : IRequestHandler<GetCardDetailQuery, CardDetailDto>
{
    private readonly IAsyncRepository<Card, int> _cardRepository;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetCardDetailQueryHandler(
        IMapper mapper, 
        IAsyncRepository<Card, int> cardRepository, 
        IIdentityService identityService)
    {
        _mapper = mapper;
        _cardRepository = cardRepository;
        _identityService = identityService;
    }

    public async Task<CardDetailDto> Handle(
        GetCardDetailQuery request, 
        CancellationToken cancellationToken)
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

        if (cardDto.Comments.Any())
        {
            var users = await _identityService.GetListAsync(card.Comments.Select(c => c.Adherent.IdentityId), cancellationToken);
            var adherentDtos = cardDto.Comments.Select(c => c.Adherent).ToList();

            MapUsers(users, adherentDtos);
        }

        if (cardDto.Activities.Any())
        {
            var users = await _identityService.GetListAsync(card.Activities.Select(c => c.Adherent.IdentityId), cancellationToken);
            var adherentDtos = cardDto.Activities.Select(c => c.Adherent).ToList();

            MapUsers(users, adherentDtos);
        }

        return cardDto;
    }

    public void MapUsers(IEnumerable<IUser> users, IEnumerable<AdherentDto> adherents)
    {
        foreach (var adherent in adherents)
        {
            var user = users.FirstOrDefault(u => u.Id == adherent.IdentityId);
            if (user == null)
                continue;

            _mapper.Map(user, adherent);
        }
    }
}