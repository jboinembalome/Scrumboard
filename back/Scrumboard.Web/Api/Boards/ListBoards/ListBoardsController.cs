using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.SharedKernel.Types;
using Scrumboard.Web.Api.Boards.Labels;
using Scrumboard.Web.Api.Users;

namespace Scrumboard.Web.Api.Boards.ListBoards;
[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/boards/{boardId:int}/[controller]")]
public class ListBoardsController(
    IMapper mapper,
    IBoardsService boardsService,
    IListBoardsService listBoardsService,
    ILabelsService labelsService,
    IIdentityService identityService) : ControllerBase
{
    /// <summary>
    /// Get lists by board id.
    /// </summary>
    /// <param name="boardId">Id of the board.</param>
    /// <param name="includeCards">Include cards.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListBoardWithCardDto>>> GetByBoardId(
        int boardId,
        bool? includeCards,
        CancellationToken cancellationToken)
    {
        var typedBoardId = new BoardId(boardId);

        if (!await boardsService.ExistsAsync(typedBoardId, cancellationToken))
        {
            return NotFound($"Board ({boardId}) not found.");
        }
        
        var listBoards = await listBoardsService.GetByBoardIdAsync(typedBoardId, includeCards, cancellationToken);
        
        var dtos = await GetListBoardDtosAsync(listBoards, includeCards, cancellationToken);
        
        return Ok(dtos);
    }
    
    private async Task<IEnumerable<ListBoardWithCardDto>> GetListBoardDtosAsync(
        IReadOnlyList<ListBoard> listBoards,
        bool? includeCards,
        CancellationToken cancellationToken)
    {
        var listBoardDtos = mapper.Map<IReadOnlyCollection<ListBoardWithCardDto>>(listBoards);
        
        if (includeCards is not true)
        {
            return listBoardDtos;
        }
        
        var cards = listBoards
            .SelectMany(x => x.Cards)
            .ToArray();
        
        var labelDtos = await GetLabelDtosAsync(cards, cancellationToken);
        var userDtos = await GetUserDtosAsync(cards, cancellationToken);

        foreach (var listBoardDto in listBoardDtos)
        {
            foreach (var cardDto in listBoardDto.Cards)
            {
                cardDto.Labels = cardDto.Labels
                    .Select(x => labelDtos.GetValueOrDefault(x.Id))
                    .Where(x => x is not null)
                    .ToList()!;
                
                cardDto.Assignees = cardDto.Assignees
                    .Select(x => userDtos.GetValueOrDefault(x.Id))
                    .Where(x => x is not null)
                    .ToList()!;
            }
        }
        
        return listBoardDtos;
    }

    private async Task<Dictionary<string, UserDto>> GetUserDtosAsync(Card[] cards, CancellationToken cancellationToken)
    {
        var userIds = cards
            .SelectMany(x => x.Assignees)
            .Select(y => (UserId)y.AssigneeId.Value)
            .ToArray();
        
        var users = await identityService.GetListAsync(userIds, cancellationToken);
        
        return mapper.Map<IEnumerable<UserDto>>(users)
            .ToDictionary(ud => ud.Id);
    }

    private async Task<Dictionary<int, LabelDto>> GetLabelDtosAsync(
        Card[] cards, 
        CancellationToken cancellationToken)
    {
        var labelIds = cards
            .SelectMany(x => x.Labels)
            .Select(y => y.LabelId)
            .ToArray();
        
        var labels = await labelsService.GetAsync(labelIds, cancellationToken);
        
        return mapper.Map<IEnumerable<LabelDto>>(labels)
            .ToDictionary(ld => ld.Id);
    }
}
