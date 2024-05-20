using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Cards.Specifications;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Queries.GetCardDetail;

internal sealed class GetCardDetailQueryHandler(
    IMapper mapper,
    IAsyncRepository<Card, int> cardRepository,
    IIdentityService identityService)
    : IRequestHandler<GetCardDetailQuery, CardDetailDto>
{
    public async Task<CardDetailDto> Handle(
        GetCardDetailQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new CardWithAllSpec(request.CardId);
        var card = await cardRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (card == null)
            throw new NotFoundException(nameof(Card), request.CardId);

        var cardDto = mapper.Map<CardDetailDto>(card);

        if (cardDto.Adherents.Any())
        {
            var users = await identityService.GetListAsync(card.Adherents.Select(a => a.IdentityId), cancellationToken);
            mapper.Map(users, cardDto.Adherents);
        }

        if (cardDto.Comments.Any())
        {
            var users = await identityService.GetListAsync(card.Comments.Select(c => c.Adherent.IdentityId), cancellationToken);
            var adherentDtos = cardDto.Comments.Select(c => c.Adherent).ToList();

            MapUsers(users, adherentDtos);
        }

        if (cardDto.Activities.Any())
        {
            var users = await identityService.GetListAsync(card.Activities.Select(c => c.Adherent.IdentityId), cancellationToken);
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

            mapper.Map(user, adherent);
        }
    }
}
