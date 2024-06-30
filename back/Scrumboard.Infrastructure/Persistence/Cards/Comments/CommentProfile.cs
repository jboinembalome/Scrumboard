using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Domain.Cards.Comments;

namespace Scrumboard.Infrastructure.Persistence.Cards.Comments;

internal sealed class CommentProfile : Profile
{
    public CommentProfile()
    {
        // Write
        CreateMap<Comment, CommentDao>();
        
        // Read
        CreateMap<CommentDao, Comment>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
    }
}
