using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.Boards.Queries.GetBoardsByUserId;

internal sealed class GetBoardsByUserIdQueryHandler(
    IMapper mapper,
    IBoardsQueryRepository boardsQueryRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetBoardsByUserIdQuery, IEnumerable<BoardDto>>
{
    public async Task<IEnumerable<BoardDto>> Handle(
        GetBoardsByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        var boards = await boardsQueryRepository.GetByUserIdAsync(currentUserService.UserId, cancellationToken);

        return mapper.Map<IEnumerable<BoardDto>>(boards);
    }
}
