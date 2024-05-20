﻿using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Boards.Specifications;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Queries.GetBoardDetail;

internal sealed class GetBoardDetailQueryHandler(
    IMapper mapper,
    IAsyncRepository<Board, int> boardRepository,
    IIdentityService identityService)
    : IRequestHandler<GetBoardDetailQuery, BoardDetailDto>
{
    public async Task<BoardDetailDto> Handle(
        GetBoardDetailQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new BoardWithAllSpec(request.BoardId);
        var board = await boardRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (board is null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        var userIds = board.Team.Adherents.Select(a => a.IdentityId);
        var users = await identityService.GetListAsync(userIds, cancellationToken);
        
        var adherentDtos = mapper.Map<IEnumerable<AdherentDto>>(board.Team.Adherents);    

        var boardDto = mapper.Map<BoardDetailDto>(board);
        boardDto.Team.Adherents = mapper.Map(users, adherentDtos);
        boardDto.Adherent = adherentDtos.First(a => a.Id == board.Adherent.Id);

        var adherentDtosInBoard = boardDto.ListBoards
            .SelectMany(l => l.Cards
                .SelectMany(c => c.Adherents
                    .Where(a => adherentDtos
                        .Any(ad => ad.Id == a.Id))));
        
        MapUsers(users, adherentDtosInBoard);
        
        return boardDto;
    }

    private void MapUsers(IEnumerable<IUser> users, IEnumerable<AdherentDto> adherents)
    {
        foreach (var adherent in adherents)
        {
            var user = users.FirstOrDefault(u => u.Id == adherent.IdentityId);
            if (user == null)
                continue;

            mapper.Map(user, adherent);
        }
    }
}
