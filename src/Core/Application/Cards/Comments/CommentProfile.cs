using AutoMapper;
using Scrumboard.Application.Cards.Comments.Commands.CreateComment;
using Scrumboard.Application.Cards.Comments.Commands.UpdateComment;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.Comments;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CreateCommentCommand, Comment>()
            .ForMember(d => d.Card.Id, opt => opt.MapFrom(c => c.CardId));
        CreateMap<UpdateCommentCommand, Comment>();
    }
}