using AutoMapper;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Features.Boards.Commands.CreateBoard;
using Scrumboard.Application.Features.Boards.Commands.UpdateBoard;
using Scrumboard.Domain.Entities;
using Scrumboard.Domain.Enums;
using System.Linq;

namespace Scrumboard.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Board, BoardDto>();
            CreateMap<Board, CreateBoardCommand>().ReverseMap();
            CreateMap<Board, UpdateBoardCommand>().ReverseMap();
            CreateMap<Board, BoardDetailDto>();

            CreateMap<BoardSetting, BoardSettingDto>();

            CreateMap<Card, CardDto>()
                .ForMember(d => d.ChecklistItemsCount, opt => opt.MapFrom(c => c.Checklists.SelectMany(ch => ch.ChecklistItems).Count()))
                .ForMember(d => d.ChecklistItemsDoneCount, opt => opt.MapFrom(c => c.Checklists.SelectMany(ch => ch.ChecklistItems).Count(i => i.IsChecked)));

            CreateMap<CustomColor, CustomColorDto>().ConvertUsing(new CustomColorTypeConverter());

            CreateMap<Label, LabelDto>();

            CreateMap<ListBoard, ListBoardDto>();

            CreateMap<Team, TeamDto>();
        }

        public class CustomColorTypeConverter : ITypeConverter<CustomColor, CustomColorDto>
        {
            public CustomColorDto Convert(CustomColor source, CustomColorDto destination, ResolutionContext context)
            {            
                return new CustomColorDto { Name = source.ToString(), Value = (int)source };
            }
        }
    }
}
