using MediatR;
using Scrumboard.Application.Cards.Dtos;

namespace Scrumboard.Application.Cards.Comments.Queries.GetCommentsByCardId;

public sealed class GetCommentsByCardIdQuery : IRequest<IEnumerable<CommentDto>>
{
    public int CardId { get; set; }
}
