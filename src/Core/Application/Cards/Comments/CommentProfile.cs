using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Cards.Comments.Commands.CreateComment;
using Scrumboard.Application.Cards.Comments.Commands.UpdateComment;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.Comments;

internal sealed class CommentProfile : Profile
{
    public CommentProfile()
    {
        // Write
        CreateMap<CreateCommentCommand, Comment>();
        
        CreateMap<UpdateCommentCommand, Comment>();
        
        // Read
        CreateMap<Comment, CommentDto>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
    }
}