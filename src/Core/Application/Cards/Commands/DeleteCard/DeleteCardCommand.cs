using MediatR;

namespace Scrumboard.Application.Cards.Commands.DeleteCard;

public sealed class DeleteCardCommand : IRequest
{
    public int CardId { get; set; }
}