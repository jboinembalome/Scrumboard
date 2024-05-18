using MediatR;

namespace Scrumboard.Application.Cards.Commands.DeleteCard;

public class DeleteCardCommand : IRequest
{
    public int CardId { get; set; }
}