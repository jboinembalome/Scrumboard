using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Boards.Labels.Specifications;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Labels.Queries.GetLabelsByBoardId;

internal sealed class GetLabelsByBoardIdQueryHandler(
    IMapper mapper,
    IAsyncRepository<Label, int> labelRepository)
    : IRequestHandler<GetLabelsByBoardIdQuery, IEnumerable<LabelDto>>
{
    public async Task<IEnumerable<LabelDto>> Handle(
        GetLabelsByBoardIdQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new AllLabelsInBoardSpec(request.BoardId);
        
        var labels = await labelRepository.ListAsync(specification, cancellationToken);

        return mapper.Map<IEnumerable<LabelDto>>(labels);
    }
}
