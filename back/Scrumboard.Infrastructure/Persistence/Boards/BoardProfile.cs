using AutoMapper;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardProfile : Profile
{
    public BoardProfile()
    {
        // Write
        CreateMap<BoardCreation, Board>();
        CreateMap<BoardEdition, Board>();
        
        CreateMap<BoardSettingCreation, BoardSetting>();
        CreateMap<BoardSettingEdition, BoardSetting>();
    }
}
