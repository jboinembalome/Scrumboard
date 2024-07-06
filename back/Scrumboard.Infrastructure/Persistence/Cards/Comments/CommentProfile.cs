using AutoMapper;
using Scrumboard.Domain.Cards.Comments;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentProfile : Profile
{
    public CommentProfile()
    {
        // Write
        CreateMap<Comment, CommentDao>();
        
        // Read
        CreateMap<CommentDao, Comment>();
    }
}
