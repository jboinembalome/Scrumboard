using MediatR;
using Scrumboard.Application.Cards.Dtos;

namespace Scrumboard.Application.Cards.Queries.GetCardDetail;

public sealed class GetCardDetailQuery : IRequest<CardDetailDto>
{
    public int CardId { get; set; }
}