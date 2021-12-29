using Scrumboard.Application.Dto;
using Scrumboard.Application.Responses;
using System.Collections.Generic;

namespace Scrumboard.Application.Features.Cards.Commands.UpdateCard
{
    public class UpdateCardCommandResponse : BaseResponse
    {
        public UpdateCardCommandResponse() : base() { }
       
        public CardDetailDto Card { get; set; }
    }
}
