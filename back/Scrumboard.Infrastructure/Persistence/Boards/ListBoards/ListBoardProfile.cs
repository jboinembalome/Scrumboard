using AutoMapper;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Persistence.Boards.ListBoards;

internal sealed class ListBoardProfile : Profile
{
    public ListBoardProfile()
    {
        // Write
        CreateMap<ListBoard, ListBoardDao>();
        
        // Read
        CreateMap<ListBoardDao, ListBoard>();
    }
}
