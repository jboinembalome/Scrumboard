using AutoMapper;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Application.ListBoards;

internal sealed class ListBoardProfile : Profile
{
    public ListBoardProfile()
    {
        // Write
        CreateMap<ListBoardInputBase, ListBoard>()
            .IncludeAllDerived();

        CreateMap<ListBoardCreation, ListBoard>();

        CreateMap<ListBoardEdition, ListBoard>();
    }
}
