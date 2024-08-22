using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.SharedKernel.Types;
using Scrumboard.Web.Api.Boards.Labels;
using Scrumboard.Web.Api.Users;

namespace Scrumboard.Web.Api.Cards;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class CardsController(
    IMapper mapper,
    ICardsService cardsService,
    ILabelsService labelsService,
    IIdentityService identityService) : ControllerBase
{
    /// <summary>
    /// Get a card by id.
    /// </summary>
    /// <param name="id">Id of the card.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CardDetailDto>> Get(
        int id,
        CancellationToken cancellationToken)
    {
        var card = await cardsService.GetByIdAsync(new CardId(id), cancellationToken);

        var dto = mapper.Map<CardDetailDto>(card);
        dto.Labels = await GetLabelDtosAsync(card, cancellationToken);
        dto.Assignees = await GetAssigneeDtosAsync(card, cancellationToken);
        
        return Ok(dto);
    }
    
    /// <summary>
    /// Create a card.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CardDetailDto>> Create(
        CardCreationDto cardCreationDto,
        CancellationToken cancellationToken)
    {
        var cardCreation = mapper.Map<CardCreation>(cardCreationDto);
        
        var card = await cardsService.AddAsync(cardCreation, cancellationToken);

        var dto = mapper.Map<CardDetailDto>(card);
        dto.Labels = await GetLabelDtosAsync(card, cancellationToken);
        dto.Assignees = await GetAssigneeDtosAsync(card, cancellationToken);
        
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    /// <summary>
    /// Update a card.
    /// </summary>
    /// <param name="id">Id of the card.</param>
    /// <param name="cardEditionDto">Card to be updated.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CardDetailDto>> Update(
        int id, 
        CardEditionDto cardEditionDto,
        CancellationToken cancellationToken)
    {
        if (id != cardEditionDto.Id)
            return BadRequest();
        
        var cardEdition = mapper.Map<CardEdition>(cardEditionDto);
        
        var card = await cardsService.UpdateAsync(cardEdition, cancellationToken);

        var dto = mapper.Map<CardDetailDto>(card);
        dto.Labels = await GetLabelDtosAsync(card, cancellationToken);
        dto.Assignees = await GetAssigneeDtosAsync(card, cancellationToken);
        
        return Ok(dto);
    }

    /// <summary>
    /// Delete a card.
    /// </summary>
    /// <param name="id">Id of the card.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await cardsService.DeleteAsync(new CardId(id), cancellationToken);
        
        return NoContent();
    }
    
    // TODO: endpoint to move a card
    
    private async Task<IEnumerable<LabelDto>> GetLabelDtosAsync(
        Card card,
        CancellationToken cancellationToken)
    {
        if (card.Labels.Count == 0)
        {
            return [];
        }
        
        var labels = await labelsService.GetAsync(card.Labels.Select(x => x.LabelId), cancellationToken);
        
        return mapper.Map<IEnumerable<LabelDto>>(labels);
    }
    
    private async Task<IEnumerable<UserDto>> GetAssigneeDtosAsync(
        Card card,
        CancellationToken cancellationToken)
    {
        if (card.Assignees.Count == 0)
        {
            return [];
        }
        
        var users = await identityService.GetListAsync(card.Assignees.Select(x => (UserId)x.AssigneeId.Value), cancellationToken);
        
        return mapper.Map<IEnumerable<UserDto>>(users);
    }
}
