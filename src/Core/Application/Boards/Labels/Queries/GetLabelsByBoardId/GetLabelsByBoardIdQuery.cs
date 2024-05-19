﻿using MediatR;
using Scrumboard.Application.Boards.Dtos;

namespace Scrumboard.Application.Boards.Labels.Queries.GetLabelsByBoardId;

public sealed class GetLabelsByBoardIdQuery : IRequest<IEnumerable<LabelDto>>
{
    public int BoardId { get; set; }
}