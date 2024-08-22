using AutoMapper;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Web.Api.Cards;

internal sealed class CardProfile : Profile
{
    public CardProfile()
    {
        // Write
        CreateMap<CardCreationDto, CardCreation>()
            .ForMember(dest => dest.LabelIds, opt => opt.MapFrom(src => src.Labels))
            .ForMember(dest => dest.AssigneeIds, opt => opt.MapFrom(src => src.Assignees));

        CreateMap<CardEditionDto, CardEdition>()
            .ForMember(dest => dest.LabelIds, opt => opt.MapFrom(src => src.Labels))
            .ForMember(dest => dest.AssigneeIds, opt => opt.MapFrom(src => src.Assignees));
        
        // Read
        CreateMap<Card, CardDto>()
            .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.Labels))
            .ForMember(dest => dest.Assignees, opt => opt.MapFrom(src => src.Assignees));

        CreateMap<Card, CardDetailDto>()
            .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.Labels))
            .ForMember(dest => dest.Assignees, opt => opt.MapFrom(src => src.Assignees));
    }
}
