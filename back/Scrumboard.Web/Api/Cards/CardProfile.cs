using AutoMapper;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Cards;
using Scrumboard.Web.Api.Boards.Labels;
using Scrumboard.Web.Api.Users;

namespace Scrumboard.Web.Api.Cards;

internal sealed class CardProfile : Profile
{
    public CardProfile()
    {
        // Write
        CreateMap<CardCreationDto, CardCreation>()
            // TODO: MapFrom LabelIds and AssigneeIds
            .ForMember(dest => dest.LabelIds, opt => opt.MapFrom(src => src.Labels))
            .ForMember(dest => dest.AssigneeIds, opt => opt.MapFrom(src => src.Assignees));

        CreateMap<CardEditionDto, CardEdition>()
            .ForMember(dest => dest.LabelIds, opt => opt.MapFrom(src => src.Labels))
            .ForMember(dest => dest.AssigneeIds, opt => opt.MapFrom(src => src.Assignees));
        
        // Read
        CreateMap<Card, CardDto>();

        CreateMap<Card, CardDetailDto>();
        
        CreateMap<CardLabel, LabelDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LabelId));
        
        CreateMap<CardAssignee, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AssigneeId));
    }
}
