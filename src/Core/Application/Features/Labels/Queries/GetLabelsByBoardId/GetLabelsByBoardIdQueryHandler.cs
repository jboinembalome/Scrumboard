using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Features.Labels.Specifications;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Labels.Queries.GetLabelsByBoardId
{
    public class GetLabelsByBoardIdQueryHandler : IRequestHandler<GetLabelsByBoardIdQuery, IEnumerable<LabelDto>>
    {
        private readonly IAsyncRepository<Label, int> _labelRepository;
        private readonly IMapper _mapper;

        public GetLabelsByBoardIdQueryHandler(IMapper mapper, IAsyncRepository<Label, int> labelRepository)
        {
            _mapper = mapper;
            _labelRepository = labelRepository;
        }

        public async Task<IEnumerable<LabelDto>> Handle(GetLabelsByBoardIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new AllLabelsInBoardSpec(request.BoardId);
            var labels = await _labelRepository.ListAsync(specification, cancellationToken);

            return _mapper.Map<IEnumerable<LabelDto>>(labels);
        }
    }
}
