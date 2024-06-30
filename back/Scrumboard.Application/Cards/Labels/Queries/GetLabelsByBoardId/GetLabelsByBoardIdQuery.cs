using MediatR;
using Scrumboard.Application.Boards.Dtos;

namespace Scrumboard.Application.Cards.Labels.Queries.GetLabelsByBoardId;

public sealed class GetLabelsByBoardIdQuery : IRequest<IEnumerable<LabelDto>>
{
    public int BoardId { get; set; }
}
