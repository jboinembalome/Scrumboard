using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Web.Api.Boards.ListBoards;

internal sealed class ListBoardProfile : Profile
{
    public ListBoardProfile()
    {
        // Write
        CreateMap<ListBoardDto, ListBoard>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
        
        // Read
        CreateMap<ListBoard, ListBoardDto>();
    }
}
