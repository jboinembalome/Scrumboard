using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Labels.GetLabelsByBoardId;

public class GetLabelsByBoardIdQuery : IRequest<IEnumerable<LabelDto>>
{
    public int BoardId { get; set; }
}