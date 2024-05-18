using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Models;

namespace Scrumboard.Application.Cards.Commands.UpdateCard;

public class UpdateCardCommandResponse : BaseResponse
{
    public UpdateCardCommandResponse() : base() { }
       
    public CardDetailDto Card { get; set; }
}