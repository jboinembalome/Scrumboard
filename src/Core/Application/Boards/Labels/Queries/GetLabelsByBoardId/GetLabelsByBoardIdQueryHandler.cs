using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Labels.Queries.GetLabelsByBoardId;

internal sealed class GetLabelsByBoardIdQueryHandler : IRequestHandler<GetLabelsByBoardIdQuery, IEnumerable<LabelDto>>
{
    private readonly IAsyncRepository<Label, int> _labelRepository;
    private readonly IMapper _mapper;

    public GetLabelsByBoardIdQueryHandler(
        IMapper mapper, 
        IAsyncRepository<Label, int> labelRepository)
    {
        _mapper = mapper;
        _labelRepository = labelRepository;
    }

    public async Task<IEnumerable<LabelDto>> Handle(
        GetLabelsByBoardIdQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new AllLabelsInBoardSpec(request.BoardId);
        
        var labels = await _labelRepository.ListAsync(specification, cancellationToken);

        return _mapper.Map<IEnumerable<LabelDto>>(labels);
    }
}