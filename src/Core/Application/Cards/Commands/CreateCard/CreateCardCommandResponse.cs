using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Models;

namespace Scrumboard.Application.Cards.Commands.CreateCard;

public sealed class CreateCardCommandResponse : BaseResponse
{
    public CreateCardCommandResponse() : base() { }

    public CardDetailDto Card { get; set; }
}