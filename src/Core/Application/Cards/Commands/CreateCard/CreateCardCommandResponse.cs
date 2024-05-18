using Scrumboard.Application.Common.Models;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Cards.Commands.CreateCard;

public class CreateCardCommandResponse : BaseResponse
{
    public CreateCardCommandResponse() : base() { }

    public CardDetailDto Card { get; set; }
}