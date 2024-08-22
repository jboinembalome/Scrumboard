using AutoMapper;
using Scrumboard.Application.Abstractions.Cards.Comments;
using Scrumboard.Domain.Cards.Comments;

namespace Scrumboard.Web.Api.Cards.Comments;

internal sealed class CommentProfile : Profile
{
    public CommentProfile()
    {
        // Write
        CreateMap<CommentCreationDto, CommentCreation>();
        CreateMap<CommentEditionDto, CommentEdition>();
        
        // Read
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.CreatedBy));
    }
}
