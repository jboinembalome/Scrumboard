using MediatR;

namespace Scrumboard.Application.Cards.Comments.Commands.UpdateComment;

public class UpdateCommentCommand : IRequest<UpdateCommentCommandResponse>
{
    public int Id { get; set; }
    public string Message { get; set; }
}