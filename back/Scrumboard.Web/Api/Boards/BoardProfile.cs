using AutoMapper;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Domain.Boards;
using Scrumboard.Web.Api.Users;

namespace Scrumboard.Web.Api.Boards;

internal sealed class BoardProfile : Profile
{
    public BoardProfile()
    {
        // Write
        CreateMap<BoardCreationDto, BoardCreation>();
        CreateMap<BoardEditionDto, BoardEdition>();

        CreateMap<BoardSettingCreationDto, BoardSettingCreation>();
        CreateMap<BoardSettingEditionDto, BoardSettingEdition>();
        
        // Read
        CreateMap<Board, BoardDto>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.OwnerId));

        CreateMap<OwnerId, UserDto>()
            .ConstructUsing(ownerId => new UserDto
            {
                Id = ownerId.Value
            });
        
        CreateMap<BoardSetting, BoardSettingDto>();
    }
}
