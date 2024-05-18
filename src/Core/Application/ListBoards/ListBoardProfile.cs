using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.ListBoards.Dtos;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Application.ListBoards;

public class ListBoardProfile : Profile
{
    public ListBoardProfile()
    {
        // Read
        CreateMap<ListBoard, ListBoardDto>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
    }
}