using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.Users.Dtos;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.Boards.Queries.GetBoardDetail;

internal sealed class GetBoardDetailQueryHandler(
    IMapper mapper,
    IBoardsQueryRepository boardsQueryRepository,
    IIdentityService identityService)
    : IRequestHandler<GetBoardDetailQuery, BoardDetailDto>
{
    public async Task<BoardDetailDto> Handle(
        GetBoardDetailQuery request, 
        CancellationToken cancellationToken)
    {
        var board = await boardsQueryRepository.TryGetByIdAsync(request.BoardId, cancellationToken);

        if (board is null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        var userIds = board.Team.Members
            .ToHashSet();
        
        var users = await identityService.GetListAsync(userIds, cancellationToken);
        var userDtos = mapper.Map<IReadOnlyCollection<UserDto>>(users);
      
        var boardDto = mapper.Map<BoardDetailDto>(board);
        boardDto.Team.Members = userDtos;
        boardDto.Creator = userDtos.First(a => a.Id == board.CreatedBy);

        var userDtosInBoard = boardDto.ListBoards
            .SelectMany(l => l.Cards
                .SelectMany(c => c.Assignees
                    .Where(a => userDtos
                        .Any(ad => ad.Id == a.Id))))
            .ToList();
        
        MapUsers(users, userDtosInBoard);
        
        return boardDto;
    }

    private void MapUsers(IReadOnlyList<IUser> users, IReadOnlyCollection<UserDto> userDtos)
    {
        foreach (var userDto in userDtos)
        {
            var user = users.FirstOrDefault(u => u.Id == userDto.Id);
            
            if (user == null)
                continue;

            mapper.Map(user, userDto);
        }
    }
}
