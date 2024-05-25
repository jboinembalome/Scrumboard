using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Boards.Specifications;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Queries.GetBoardsByUserId;

internal sealed class GetBoardsByUserIdQueryHandler(
    IMapper mapper,
    IAsyncRepository<Board, int> boardRepository)
    : IRequestHandler<GetBoardsByUserIdQuery, IEnumerable<BoardDto>>
{
    public async Task<IEnumerable<BoardDto>> Handle(
        GetBoardsByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new BoardsByUserIdSpec(request.UserId);
        
        var boards = await boardRepository.ListAsync(specification, cancellationToken);

        return mapper.Map<IEnumerable<BoardDto>>(boards);
    }
}
