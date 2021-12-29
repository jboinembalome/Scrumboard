using MediatR;
using Scrumboard.Application.Dto;
using System.Collections.Generic;

namespace Scrumboard.Application.Features.Labels.Queries.GetLabelsByBoardId
{
    public class GetLabelsByBoardIdQuery : IRequest<IEnumerable<LabelDto>>
    {
        public int BoardId { get; set; }
    }
}
