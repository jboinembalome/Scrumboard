using AutoMapper;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.Web.Api.ListBoards;

namespace Scrumboard.Web.Api.Boards.ListBoards;

internal sealed class ListBoardProfile : Profile
{
    public ListBoardProfile()
    {
        // Read
        CreateMap<ListBoard, ListBoardWithCardDto>();
    }
}
