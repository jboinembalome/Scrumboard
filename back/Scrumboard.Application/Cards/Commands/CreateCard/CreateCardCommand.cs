using MediatR;

namespace Scrumboard.Application.Cards.Commands.CreateCard;

public sealed class CreateCardCommand : IRequest<CreateCardCommandResponse>
{
    public int ListBoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
}
