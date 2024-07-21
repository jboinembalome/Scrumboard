using AutoMapper;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

namespace Scrumboard.Web.Api.Cards;

internal sealed class CardProfile : Profile
{
    public CardProfile()
    {
        // Write
        CreateMap<CardCreationModel, CardCreation>()
            .ForMember(dest => dest.LabelIds, opt => opt.MapFrom(src => src.Labels))
            .ForMember(dest => dest.AssigneeIds, opt => opt.MapFrom(src => src.Assignees));

        CreateMap<CardEditionModel, CardEdition>()
            .ForMember(dest => dest.LabelIds, opt => opt.MapFrom(src => src.Labels))
            .ForMember(dest => dest.AssigneeIds, opt => opt.MapFrom(src => src.Assignees));
        
        // Read
        CreateMap<Card, CardDto>()
            .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.LabelIds))
            .ForMember(dest => dest.Assignees, opt => opt.MapFrom(src => src.AssigneeIds));

        CreateMap<Card, CardDetailDto>()
            .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.LabelIds))
            .ForMember(dest => dest.Assignees, opt => opt.MapFrom(src => src.AssigneeIds));
    }
}
