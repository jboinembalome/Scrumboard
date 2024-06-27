using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Boards.Commands.CreateBoard;
using Scrumboard.Application.Boards.Commands.DeleteBoard;
using Scrumboard.Application.Boards.Commands.UpdateBoard;
using Scrumboard.Application.Boards.Commands.UpdatePinnedBoard;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Boards.Labels.Queries.GetLabelsByBoardId;
using Scrumboard.Application.Boards.Queries.GetBoardDetail;
using Scrumboard.Application.Boards.Queries.GetBoardsByUserId;
using Scrumboard.Infrastructure.Abstractions.Common;

namespace Scrumboard.Web.Api.Boards;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class BoardsController(
    ISender mediator,
    ICurrentUserService currentUserService) : ControllerBase
{
    /// <summary>
    /// Get the boards of the current user.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BoardDto>>>Get()
    {
        var dtos = await mediator.Send(new GetBoardsByUserIdQuery { UserId = currentUserService.UserId });

        return Ok(dtos);
    }

    /// <summary>
    /// Get a board by id.
    /// </summary>
    /// <param name="id">Id of the board.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<BoardDetailDto>> Get(int id)
    {
        var dto = await mediator.Send(new GetBoardDetailQuery { BoardId = id });

        return Ok(dto);
    }

    /// <summary>
    /// Create a board.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateBoardCommandResponse>> Create()
    {
        var response = await mediator.Send(new CreateBoardCommand { CreatorId = currentUserService.UserId });

        return CreatedAtAction(nameof(Get), new { id = response.Board.Id }, response);
    }

    /// <summary>
    /// Update a board.
    /// </summary>
    /// <param name="id">Id of the board.</param>
    /// <param name="command">Board to be updated.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(int id, UpdateBoardCommand command)
    {
        if (id != command.BoardId)
            return BadRequest();

        var dto = await mediator.Send(command);

        return Ok(dto);
    }

    /// <summary>
    /// Update the pinned board.
    /// </summary>
    /// <param name="id">Id of the board.</param>
    /// <param name="command">Pinned board to be updated.</param>
    /// <returns></returns>
    [HttpPut("{id}/pinned")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdatePinned(int id, UpdatePinnedBoardCommand command)
    {
        if (id != command.BoardId)
            return BadRequest();

        await mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Delete a board.
    /// </summary>
    /// <param name="id">Id of the board.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteBoardCommand { BoardId = id });

        return NoContent();
    }

    /// <summary>
    /// Get labels by board id.
    /// </summary>
    /// <param name="id">Id of the board.</param>
    /// <returns></returns>
    [HttpGet("{id}/labels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LabelDto>>> GetByBoardId(int id)
    {
        var dto = await mediator.Send(new GetLabelsByBoardIdQuery { BoardId = id });

        return Ok(dto);
    }
}
