using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Boards.Labels.Specifications;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Labels.Queries.GetLabelsByBoardId;

internal sealed class GetLabelsByBoardIdQueryHandler(
    IMapper mapper,
    IAsyncRepository<Board, int> boardRepository)
    : IRequestHandler<GetLabelsByBoardIdQuery, IEnumerable<LabelDto>>
{
    public async Task<IEnumerable<LabelDto>> Handle(
        GetLabelsByBoardIdQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new AllLabelsInBoardSpec(request.BoardId);
        var board = await boardRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (board is null)
        {
            return [];
        }

        var labels = board.ListBoards
            .SelectMany(x => x.Cards
                .SelectMany(y => y.Labels))
            .ToHashSet();

        return mapper.Map<IEnumerable<LabelDto>>(labels);
    }
}
