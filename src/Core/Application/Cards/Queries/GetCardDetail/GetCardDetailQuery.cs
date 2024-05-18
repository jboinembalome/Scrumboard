using MediatR;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Cards.Queries.GetCardDetail;

public class GetCardDetailQuery : IRequest<CardDetailDto>
{
    public int CardId { get; set; }
}