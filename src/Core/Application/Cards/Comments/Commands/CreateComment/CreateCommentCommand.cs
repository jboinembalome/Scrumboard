using MediatR;

namespace Scrumboard.Application.Cards.Comments.Commands.CreateComment;

public sealed class CreateCommentCommand : IRequest<CreateCommentCommandResponse>
{
    public string Message { get; set; }

    public int CardId { get; set; }
}