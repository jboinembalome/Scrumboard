using AutoMapper;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Features.Boards.Commands.CreateBoard;
using Scrumboard.Application.Features.Boards.Commands.UpdateBoard;
using Scrumboard.Domain.Entities;
using Scrumboard.Domain.ValueObjects;
using System.Linq;

namespace Scrumboard.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Adherent, AdherentDto>();

            CreateMap<Board, BoardDto>();
            CreateMap<Board, CreateBoardCommand>().ReverseMap();
            CreateMap<Board, UpdateBoardCommand>().ReverseMap();
            CreateMap<Board, BoardDetailDto>();

            CreateMap<BoardSetting, BoardSettingDto>();

            CreateMap<Card, CardDto>()
                .ForMember(d => d.ChecklistItemsCount, opt => opt.MapFrom(c => c.Checklists.SelectMany(ch => ch.ChecklistItems).Count()))
                .ForMember(d => d.ChecklistItemsDoneCount, opt => opt.MapFrom(c => c.Checklists.SelectMany(ch => ch.ChecklistItems).Count(i => i.IsChecked)));

            CreateMap<Colour, ColourDto>()
                .ForMember(d => d.Colour, opt => opt.MapFrom(c => c.Code));

            CreateMap<Label, LabelDto>();

            CreateMap<ListBoard, ListBoardDto>();

            CreateMap<Team, TeamDto>();
        }

    }
}
