using Scrumboard.Application.Dto;
using Scrumboard.Application.Responses;

namespace Scrumboard.Application.Features.Cards.Commands.CreateCard
{
    public class CreateCardCommandResponse : BaseResponse
    {
        public CreateCardCommandResponse() : base() { }

        public CardDetailDto Card { get; set; }
    }
}
