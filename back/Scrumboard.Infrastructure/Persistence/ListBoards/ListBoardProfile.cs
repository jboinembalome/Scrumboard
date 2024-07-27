using AutoMapper;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Infrastructure.Persistence.ListBoards;

internal sealed class ListBoardProfile : Profile
{
    public ListBoardProfile()
    {
        // Write
        CreateMap<ListBoardCreation, ListBoardDao>();

        CreateMap<ListBoardEdition, ListBoardDao>();
        
        // Read
        CreateMap<ListBoardDao, ListBoard>();
    }
}
