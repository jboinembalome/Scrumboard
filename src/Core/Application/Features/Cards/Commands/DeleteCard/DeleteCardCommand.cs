using MediatR;

namespace Scrumboard.Application.Features.Cards.Commands.DeleteCard
{
    public class DeleteCardCommand : IRequest
    {
        public int CardId { get; set; }
    }
}
