using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Web.Api.ListBoards;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class ListBoardsController(
    IMapper mapper,
    IListBoardsService listBoardsService) : ControllerBase
{
    /// <summary>
    /// Get a list by id.
    /// </summary>
    /// <param name="id">Id of the list.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ListBoardDto>> Get(
        int id,
        CancellationToken cancellationToken)
    {
        var listBoard = await listBoardsService.GetByIdAsync(new ListBoardId(id), cancellationToken);

        var dto = mapper.Map<ListBoardDto>(listBoard);
        
        return Ok(dto);
    }
    
    /// <summary>
    /// Create a list.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ListBoardDto>> Create(
        ListBoardCreationDto listBoardCreationDto,
        CancellationToken cancellationToken)
    {
        var listBoardCreation = mapper.Map<ListBoardCreation>(listBoardCreationDto);
        
        var listBoard = await listBoardsService.AddAsync(listBoardCreation, cancellationToken);

        var dto = mapper.Map<ListBoardDto>(listBoard);
        
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    /// <summary>
    /// Update a list.
    /// </summary>
    /// <param name="id">Id of the list.</param>
    /// <param name="listBoardEditionDto">List to be updated.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ListBoardDto>> Update(
        int id, 
        ListBoardEditionDto listBoardEditionDto,
        CancellationToken cancellationToken)
    {
        if (id != listBoardEditionDto.Id)
            return BadRequest();
        
        var listBoardEdition = mapper.Map<ListBoardEdition>(listBoardEditionDto);
        
        var listBoard = await listBoardsService.UpdateAsync(listBoardEdition, cancellationToken);

        var dto = mapper.Map<ListBoardDto>(listBoard);
        
        return Ok(dto);
    }

    /// <summary>
    /// Delete a list.
    /// </summary>
    /// <param name="id">Id of the list.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await listBoardsService.DeleteAsync(new ListBoardId(id), cancellationToken);
        
        return NoContent();
    }
}
