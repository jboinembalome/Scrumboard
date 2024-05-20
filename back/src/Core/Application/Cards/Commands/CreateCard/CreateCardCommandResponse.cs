using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Cards.Commands.CreateCard;

public sealed class CreateCardCommandResponse : BaseResponse
{
    public CardDetailDto Card { get; set; }
}
