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
        
        CreateMap<BoardSettingCreationDto, BoardSetting>();
        CreateMap<BoardSettingEditionDto, BoardSetting>();
        
        // Read
        CreateMap<Board, BoardDto>();
        CreateMap<Board, BoardDetailDto>();
        
        CreateMap<BoardSetting, BoardSettingDto>();
    }
}
