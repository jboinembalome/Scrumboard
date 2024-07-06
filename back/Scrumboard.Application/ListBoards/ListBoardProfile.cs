using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.ListBoards.Dtos;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Application.ListBoards;

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
