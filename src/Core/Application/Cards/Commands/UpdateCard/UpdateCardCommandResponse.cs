using Scrumboard.Application.Common.Models;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Cards.Commands.UpdateCard;

public class UpdateCardCommandResponse : BaseResponse
{
    public UpdateCardCommandResponse() : base() { }
       
    public CardDetailDto Card { get; set; }
}