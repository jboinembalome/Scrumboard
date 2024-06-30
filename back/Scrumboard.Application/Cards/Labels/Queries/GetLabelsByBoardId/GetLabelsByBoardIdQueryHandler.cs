using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Application.Cards.Labels.Queries.GetLabelsByBoardId;

internal sealed class GetLabelsByBoardIdQueryHandler(
    IMapper mapper,
    ILabelsQueryRepository labelsQueryRepository)
    : IRequestHandler<GetLabelsByBoardIdQuery, IEnumerable<LabelDto>>
{
    public async Task<IEnumerable<LabelDto>> Handle(
        GetLabelsByBoardIdQuery request, 
        CancellationToken cancellationToken)
    {
        var labels = await labelsQueryRepository.GetByBoardIdAsync(request.BoardId, cancellationToken);

        return mapper.Map<IEnumerable<LabelDto>>(labels);
    }
}
