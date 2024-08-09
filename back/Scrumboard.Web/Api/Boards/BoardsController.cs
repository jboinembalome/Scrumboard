using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Web.Api.Boards;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class BoardsController(
    IMapper mapper,
    IBoardsService boardsService) : ControllerBase
{
    /// <summary>
    /// Get the boards of the current user.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BoardDto>>>Get(
        CancellationToken cancellationToken)
    {
        var boards = await boardsService.GetAsync(cancellationToken);
        
        var dtos = mapper.Map<IEnumerable<BoardDto>>(boards);
        
        return Ok(dtos);
    }

    /// <summary>
    /// Get a board by id.
    /// </summary>
    /// <param name="id">Id of the board.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<BoardDto>> Get(
        int id, 
        CancellationToken cancellationToken)
    {
        var board = await boardsService.GetByIdAsync(new BoardId(id), cancellationToken);
        
        var dto = mapper.Map<BoardDto>(board);

        return Ok(dto);
    }

    /// <summary>
    /// Create a board.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<BoardDto>> Create(
        BoardCreationDto boardCreationDto, 
        CancellationToken cancellationToken)
    {
        var boardCreation = mapper.Map<BoardCreation>(boardCreationDto);
        
        var board = await boardsService.AddAsync(boardCreation, cancellationToken);
        
        var dto = mapper.Map<BoardDto>(board);

        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    /// <summary>
    /// Update a board.
    /// </summary>
    /// <param name="id">Id of the board.</param>
    /// <param name="boardEditionDto">Board to be updated.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BoardDto>> Update(
        int id, 
        BoardEditionDto boardEditionDto,
        CancellationToken cancellationToken)
    {
        if (id != boardEditionDto.Id)
            return BadRequest();
        
        var boardEdition = mapper.Map<BoardEdition>(boardEditionDto);
        
        var board = await boardsService.UpdateAsync(boardEdition, cancellationToken);
        
        var dto = mapper.Map<BoardDto>(board);

        return Ok(dto);
    }

    /// <summary>
    /// Delete a board.
    /// </summary>
    /// <param name="id">Id of the board.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(
        int id, 
        CancellationToken cancellationToken)
    {
        await boardsService.DeleteAsync(new BoardId(id), cancellationToken);

        return NoContent();
    }
}
