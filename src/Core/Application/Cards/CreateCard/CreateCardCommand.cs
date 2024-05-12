using MediatR;

namespace Scrumboard.Application.Cards.CreateCard;

public class CreateCardCommand : IRequest<CreateCardCommandResponse>
{
    public int ListBoardId { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
}