using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Abstractions.Cards.Comments;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;
using Scrumboard.Web.Api.Users;

namespace Scrumboard.Web.Api.Cards.Comments;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/cards/{cardId:int}/[controller]")]
public class CommentsController(
    IMapper mapper,
    ICardsService cardsService,
    ICommentsService commentsService,
    IIdentityService identityService,
    IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>
    /// Get card comments.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetAll(
        int cardId, 
        CancellationToken cancellationToken)
    {
        var typedCardId = new CardId(cardId);

        if (!await cardsService.ExistsAsync(typedCardId, cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }

        var comments = await commentsService.GetByCardIdAsync(typedCardId, cancellationToken);
        
        var dtos = await GetCommentDtosAsync(comments, cancellationToken);
        
        return Ok(dtos);
    }
    
    /// <summary>
    /// Get card comment by id.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="id">Id of the comment.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CommentDto>> Get(
        int cardId, 
        int id, 
        CancellationToken cancellationToken)
    {
        if (!await cardsService.ExistsAsync(new CardId(cardId), cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }

        var comment = await commentsService.GetByIdAsync(new CommentId(id), cancellationToken);
        
        var dto = await GetCommentDtoAsync(comment, cancellationToken);
        
        return Ok(dto);
    }
    
    /// <summary>
    /// Create a comment on a card.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="commentCreationDto">Comment to be added.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CommentDto>> Create(
        int cardId, 
        CommentCreationDto commentCreationDto,
        CancellationToken cancellationToken)
    {
        if (!await cardsService.ExistsAsync(new CardId(cardId), cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }

        var commentCreation = mapper.Map<CommentCreation>(commentCreationDto);
        
        var comment = await commentsService.AddAsync(commentCreation, cancellationToken);
        
        await unitOfWork.CommitAsync(cancellationToken);
        
        var dto = await GetCommentDtoAsync(comment, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    /// <summary>
    /// Update a comment on a card.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="id">Id of the comment.</param>
    /// <param name="commentEditionDto">Comment to be updated.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(
        int cardId, 
        int id, 
        CommentEditionDto commentEditionDto,
        CancellationToken cancellationToken)
    {
        if (id != commentEditionDto.Id)
            return BadRequest();
        
        if (!await cardsService.ExistsAsync(new CardId(cardId), cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }

        var commentEdition = mapper.Map<CommentEdition>(commentEditionDto);
        
        var comment = await commentsService.UpdateAsync(commentEdition, cancellationToken);
        
        await unitOfWork.CommitAsync(cancellationToken);

        var dto = await GetCommentDtoAsync(comment, cancellationToken);

        return Ok(dto);
    }

    /// <summary>
    /// Delete a comment on a card.
    /// </summary>
    /// <param name="cardId">Id of the card.</param>
    /// <param name="id">Id of the comment.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(
        int cardId, 
        int id,
        CancellationToken cancellationToken)
    {
        if (!await cardsService.ExistsAsync(new CardId(cardId), cancellationToken))
        {
            return NotFound($"Card ({cardId}) not found.");
        }
        
        await commentsService.DeleteAsync(new CommentId(id), cancellationToken);
        
        await unitOfWork.CommitAsync(cancellationToken);

        return NoContent();
    }
    
    private async Task<IReadOnlyCollection<CommentDto>> GetCommentDtosAsync(
        IReadOnlyCollection<Comment> comments,
        CancellationToken cancellationToken)
    {
        var commentDtos = mapper.Map<IEnumerable<CommentDto>>(comments).ToList();
        
        var users = await identityService
            .GetListAsync(comments
                .Select(a => a.CreatedBy), cancellationToken);
        
        var userDtos = commentDtos.Select(c => c.User).ToList();

        MapUsers(users, userDtos);

        return commentDtos;
    }
    
    private async Task<CommentDto> GetCommentDtoAsync(
        Comment comment,
        CancellationToken cancellationToken)
    {
        var commentDto = mapper.Map<CommentDto>(comment);
        
        var user = await identityService.GetUserAsync(comment.CreatedBy, cancellationToken);

        mapper.Map(user, commentDto.User);

        return commentDto;
    }
    
    private void MapUsers(IReadOnlyList<IUser> users, IEnumerable<UserDto> userDtos)
    {
        foreach (var userDto in userDtos)
        {
            var user = users.FirstOrDefault(u => u.Id == userDto.Id);

            if (user is null)
            {
                continue;
            }

            mapper.Map(user, userDto);
        }
    }
}
