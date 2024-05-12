using Scrumboard.Application.Dto;
using Scrumboard.Application.Responses;

namespace Scrumboard.Application.Cards.UpdateCard;

public class UpdateCardCommandResponse : BaseResponse
{
    public UpdateCardCommandResponse() : base() { }
       
    public CardDetailDto Card { get; set; }
}