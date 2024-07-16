using AutoMapper;
using Scrumboard.Domain.Cards.Comments;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentProfile : Profile
{
    public CommentProfile()
    {
        // Write
        CreateMap<CommentCreation, CommentDao>();
        CreateMap<CommentEdition, CommentDao>();
        
        // Read
        CreateMap<CommentDao, Comment>();
    }
}
