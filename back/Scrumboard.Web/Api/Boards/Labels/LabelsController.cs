using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Web.Api.Boards.Labels;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/boards/{boardId:int}/[controller]")]
public sealed class LabelsController(
    IMapper mapper,
    IBoardsService boardsService,
    ILabelsService labelsService) : ControllerBase
{
    /// <summary>
    /// Get labels by board id.
    /// </summary>
    /// <param name="boardId">Id of the board.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LabelDto>>> GetByBoardId(
        int boardId, 
        CancellationToken cancellationToken)
    {
        if (!await boardsService.ExistsAsync(boardId, cancellationToken))
        {
            return NotFound($"Board ({boardId}) not found.");
        }
        
        var labels = await labelsService.GetByBoardIdAsync(boardId, cancellationToken);

        var dtos = mapper.Map<IEnumerable<LabelDto>>(labels);
        
        return Ok(dtos);
    }
    
    /// <summary>
    /// Get board label by id.
    /// </summary>
    /// <param name="boardId">Id of the board.</param>
    /// <param name="id">Id of the label.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LabelDto>> Get(
        int boardId, 
        int id, 
        CancellationToken cancellationToken)
    {
        if (!await boardsService.ExistsAsync(boardId, cancellationToken))
        {
            return NotFound($"Board ({boardId}) not found.");
        }

        var label = await labelsService.GetByIdAsync(id, cancellationToken);

        var dto = mapper.Map<LabelDto>(label);
        
        return Ok(dto);
    }
    
    /// <summary>
    /// Create a label on a board.
    /// </summary>
    /// <param name="boardId">Id of the board.</param>
    /// <param name="labelCreationModel">Label to be added.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<LabelDto>> Create(
        int boardId, 
        LabelCreationModel labelCreationModel,
        CancellationToken cancellationToken)
    {
        if (!await boardsService.ExistsAsync(boardId, cancellationToken))
        {
            return NotFound($"Board ({boardId}) not found.");
        }

        var labelCreation = mapper.Map<LabelCreation>(labelCreationModel);
        
        var label = await labelsService.AddAsync(labelCreation, cancellationToken);

        var dto = mapper.Map<LabelDto>(label);

        return CreatedAtAction(nameof(Get), new { BoardId = boardId, dto.Id }, dto);
    }

    /// <summary>
    /// Update a comment on a card.
    /// </summary>
    /// <param name="boardId">Id of the board.</param>
    /// <param name="id">Id of the label.</param>
    /// <param name="labelEditionModel">Label to be updated.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LabelDto>> Update(
        int boardId, 
        int id, 
        LabelEditionModel labelEditionModel,
        CancellationToken cancellationToken)
    {
        if (id != labelEditionModel.Id)
            return BadRequest();
        
        if (!await boardsService.ExistsAsync(boardId, cancellationToken))
        {
            return NotFound($"Board ({boardId}) not found.");
        }

        var labelEdition = mapper.Map<LabelEdition>(labelEditionModel);
        
        var label = await labelsService.UpdateAsync(labelEdition, cancellationToken);

        var dto = mapper.Map<LabelDto>(label);

        return Ok(dto);
    }
    
    /// <summary>
    /// Delete a label on a board.
    /// </summary>
    /// <remarks>Delete the label from all cards in the board.</remarks>
    /// <param name="boardId">Id of the board.</param>
    /// <param name="id">Id of the label.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(
        int boardId, 
        int id, 
        CancellationToken cancellationToken)
    {
        if (!await boardsService.ExistsAsync(boardId, cancellationToken))
        {
            return NotFound($"Board ({boardId}) not found.");
        }

        await labelsService.DeleteAsync(id, cancellationToken);
        
        return NoContent();
    }
}
