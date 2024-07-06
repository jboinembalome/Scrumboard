using AutoMapper;
using Scrumboard.Application.Boards.Commands.CreateBoard;
using Scrumboard.Application.Boards.Commands.UpdateBoard;
using Scrumboard.Application.Boards.Commands.UpdatePinnedBoard;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Boards;

internal sealed class BoardProfile : Profile
{
    public BoardProfile()
    {
        // Write
        CreateMap<CreateBoardCommand, Board>();
        
        CreateMap<UpdateBoardCommand, Board>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BoardId));
        
        CreateMap<UpdatePinnedBoardCommand, Board>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BoardId));
        
        CreateMap<BoardSettingDto, BoardSetting>();
        
        // Read
        CreateMap<Board, BoardDto>()
            .ForMember(dest => dest.Initials, opt => opt.MapFrom(src => GetInitials(src)))
            .ForMember(dest => dest.LastActivity, opt => opt.MapFrom(src => src.LastModifiedDate))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Team.Members.Count));
        CreateMap<Board, BoardDetailDto>();
        
        CreateMap<BoardSetting, BoardSettingDto>();
    }
    
    // Gets only the first two characters of the initials.
    private static string GetInitials(Board board)
    {
        var initials = board.GetInitials();

        return !string.IsNullOrEmpty(initials) && initials.Length <= 2 
            ? initials 
            : initials.Substring(0, 2);
    }
}
