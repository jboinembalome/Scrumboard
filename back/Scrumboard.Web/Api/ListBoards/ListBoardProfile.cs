using AutoMapper;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Domain.ListBoards;

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
    }
}
