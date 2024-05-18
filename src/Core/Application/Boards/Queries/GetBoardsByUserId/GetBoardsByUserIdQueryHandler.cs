using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Queries.GetBoardsByUserId;

public class GetBoardsByUserIdQueryHandler : IRequestHandler<GetBoardsByUserIdQuery, IEnumerable<BoardDto>>
{
    private readonly IAsyncRepository<Board, int> _boardRepository;
    private readonly IMapper _mapper;

    public GetBoardsByUserIdQueryHandler(IMapper mapper, IAsyncRepository<Board, int> boardRepository)
    {
        _mapper = mapper;
        _boardRepository = boardRepository;
    }

    public async Task<IEnumerable<BoardDto>> Handle(GetBoardsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new BoardsByUserIdSpec(request.UserId);
        var boards = await _boardRepository.ListAsync(specification, cancellationToken);

        return _mapper.Map<IEnumerable<BoardDto>>(boards);
    }
}