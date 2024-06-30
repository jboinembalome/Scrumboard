using AutoMapper;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardProfile : Profile
{
    public BoardProfile()
    {
        // Write
        CreateMap<Board, BoardDao>();
        CreateMap<BoardSetting, BoardSettingDao>();
      
        // Read
        CreateMap<BoardDao, Board>();
        CreateMap<BoardSettingDao, BoardSetting>();
    }
}
