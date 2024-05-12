using MediatR;

namespace Scrumboard.Application.Cards.DeleteCard;

public class DeleteCardCommand : IRequest
{
    public int CardId { get; set; }
}