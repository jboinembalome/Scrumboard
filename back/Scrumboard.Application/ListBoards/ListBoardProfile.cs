using AutoMapper;
using Scrumboard.Application.ListBoards.Dtos;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Application.ListBoards;

internal sealed class ListBoardProfile : Profile
{
    public ListBoardProfile()
    {
        // Write
        CreateMap<ListBoardDto, ListBoard>();
        
        // Read
        CreateMap<ListBoard, ListBoardDto>();
    }
}
