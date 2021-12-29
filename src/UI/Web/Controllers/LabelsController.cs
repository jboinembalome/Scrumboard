using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Features.Labels.Queries.GetLabelsByBoardId;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrumboard.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class LabelsController : ApiControllerBase
    {
        public LabelsController()
        {
        }

        /// <summary>
        /// Get labels by board id.
        /// </summary>
        /// <param name="boardId">Id of the board.</param>
        /// <returns></returns>
        [HttpGet("boards/{boardId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LabelDto>>> GetByBoardId(int boardId)
        {
            var dto = await Mediator.Send(new GetLabelsByBoardIdQuery { BoardId = boardId });

            return Ok(dto);
        }
    }
}
