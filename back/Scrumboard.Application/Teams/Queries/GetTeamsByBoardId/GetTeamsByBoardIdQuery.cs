using MediatR;
using Scrumboard.Application.Teams.Dtos;

namespace Scrumboard.Application.Teams.Queries.GetTeamsByBoardId;

public sealed class GetTeamsByBoardIdQuery : IRequest<TeamDto>
{
    public int BoardId { get; set; }
}
