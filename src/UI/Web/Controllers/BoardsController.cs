using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Features.Boards.Commands.CreateBoard;
using Scrumboard.Application.Features.Boards.Commands.DeleteBoard;
using Scrumboard.Application.Features.Boards.Commands.UpdateBoard;
using Scrumboard.Application.Features.Boards.Commands.UpdatePinnedBoard;
using Scrumboard.Application.Features.Boards.Queries.GetBoardDetail;
using Scrumboard.Application.Features.Boards.Queries.GetBoardsByUserId;
using Scrumboard.Application.Features.Labels.Queries.GetLabelsByBoardId;
using Scrumboard.Application.Interfaces.Common;
using Scrumboard.Infrastructure.Notification.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrumboard.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class BoardsController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubContext<BoardHub> _boardHubContext;

        public BoardsController(ICurrentUserService currentUserService, IHubContext<BoardHub> boardHubContext)
        {
            _currentUserService = currentUserService;
            _boardHubContext = boardHubContext;
        }

        /// <summary>
        /// Get the boards of the current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BoardDto>>>Get()
        {
            var dtos = await Mediator.Send(new GetBoardsByUserIdQuery { UserId = _currentUserService.UserId });

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
            var dto = await Mediator.Send(new GetBoardDetailQuery { BoardId = id });

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
            var response = await Mediator.Send(new CreateBoardCommand { UserId = _currentUserService.UserId });

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

            var dto = await Mediator.Send(command);

            await _boardHubContext.Clients.All.SendAsync("UpdatedBoard", dto);

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

            await Mediator.Send(command);

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
            await Mediator.Send(new DeleteBoardCommand { BoardId = id });

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
            var dto = await Mediator.Send(new GetLabelsByBoardIdQuery { BoardId = id });

            return Ok(dto);
        }
    }
}
