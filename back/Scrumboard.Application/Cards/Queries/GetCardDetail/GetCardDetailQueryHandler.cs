using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Application.Cards.Queries.GetCardDetail;

internal sealed class GetCardDetailQueryHandler(
    IMapper mapper,
    ICardsQueryRepository cardsQueryRepository,
    IIdentityService identityService)
    : IRequestHandler<GetCardDetailQuery, CardDetailDto>
{
    public async Task<CardDetailDto> Handle(
        GetCardDetailQuery request, 
        CancellationToken cancellationToken)
    {
        var card = await cardsQueryRepository.TryGetByIdAsync(request.CardId, cancellationToken);

        if (card is null)
            throw new NotFoundException(nameof(Card), request.CardId);

        var cardDto = mapper.Map<CardDetailDto>(card);

        if (!cardDto.Assignees.Any())
        {
            return cardDto;
        }

        var assigneeIds = card.Assignees
            .ToHashSet();
        
        var users = await identityService.GetListAsync(assigneeIds, cancellationToken);
        mapper.Map(users, cardDto.Assignees);

        return cardDto;
    }
}
