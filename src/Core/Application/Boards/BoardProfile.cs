using AutoMapper;
using Scrumboard.Application.Boards.Commands.CreateBoard;
using Scrumboard.Application.Boards.Commands.UpdateBoard;
using Scrumboard.Application.Boards.Commands.UpdatePinnedBoard;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Boards;

public class BoardProfile : Profile
{
    public BoardProfile()
    {
        CreateMap<CreateBoardCommand, Board>()
            .ForMember(dest => dest.Adherent.IdentityId, opt => opt.MapFrom(src => src.UserId));
        CreateMap<UpdateBoardCommand, Board>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BoardId));
        CreateMap<UpdatePinnedBoardCommand, Board>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BoardId));
    }
}