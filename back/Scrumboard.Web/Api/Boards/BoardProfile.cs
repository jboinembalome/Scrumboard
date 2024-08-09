using AutoMapper;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

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
        CreateMap<Board, BoardDto>();
        
        CreateMap<BoardSetting, BoardSettingDto>();
    }
}
