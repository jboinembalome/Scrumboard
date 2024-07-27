using AutoMapper;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Web.Api.ListBoards;

internal sealed class ListBoardProfile : Profile
{
    public ListBoardProfile()
    {
        // Write
        CreateMap<ListBoardCreationDto, ListBoardCreation>();

        CreateMap<ListBoardEditionDto, ListBoardEdition>();
        
        // Read
        CreateMap<ListBoard, ListBoardDto>();
        CreateMap<ListBoard, ListBoardDetailDto>();
    }
}
