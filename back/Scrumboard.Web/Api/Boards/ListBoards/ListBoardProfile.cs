using AutoMapper;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Web.Api.Boards.ListBoards;

internal sealed class ListBoardProfile : Profile
{
    public ListBoardProfile()
    {
        // Read
        CreateMap<ListBoard, ListBoardWithCardDto>();
    }
}
