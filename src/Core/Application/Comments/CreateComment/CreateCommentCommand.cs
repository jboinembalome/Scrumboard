using MediatR;

namespace Scrumboard.Application.Comments.CreateComment;

public class CreateCommentCommand : IRequest<CreateCommentCommandResponse>
{
    public string Message { get; set; }

    public int CardId { get; set; }
}