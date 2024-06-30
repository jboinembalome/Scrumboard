using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Persistence.Boards.ListBoards;

internal sealed class ListBoardProfile : Profile
{
    public ListBoardProfile()
    {
        // Write
        CreateMap<ListBoard, ListBoardDao>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
        
        // Read
        CreateMap<ListBoardDao, ListBoard>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
    }
}
