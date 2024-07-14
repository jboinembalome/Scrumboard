using AutoMapper;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardProfile : Profile
{
    public BoardProfile()
    {
        // Write
        CreateMap<BoardCreation, BoardDao>();
        CreateMap<BoardEdition, BoardDao>();
        
        CreateMap<BoardSettingCreation, BoardSettingDao>();
        CreateMap<BoardSetting, BoardSettingDao>();
      
        // Read
        CreateMap<BoardDao, Board>();
        
        CreateMap<BoardSettingDao, BoardSetting>();
    }
}
