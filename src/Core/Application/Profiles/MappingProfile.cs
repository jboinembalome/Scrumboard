using AutoMapper;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Extensions;
using Scrumboard.Application.Features.Boards.Commands.CreateBoard;
using Scrumboard.Application.Features.Boards.Commands.UpdateBoard;
using Scrumboard.Application.Features.Boards.Commands.UpdatePinnedBoard;
using Scrumboard.Domain.Entities;
using Scrumboard.Domain.ValueObjects;
using System;
using System.Linq;

namespace Scrumboard.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Adherent, AdherentDto>();

            CreateMap<Board, BoardDto>()
                .ForMember(d => d.Initials, opt => opt.MapFrom(c => GetInitials(c.Name)))
                .ForMember(d => d.LastActivity, opt => opt.MapFrom(c => c.LastModifiedDate))
                .ForMember(d => d.Members, opt => opt.MapFrom(c => c.Team.Adherents.Count));
            CreateMap<Board, CreateBoardCommand>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(c => c.Adherent.IdentityGuid))
                .ReverseMap();
            CreateMap<Board, UpdateBoardCommand>()
                .ForMember(d => d.BoardId, opt => opt.MapFrom(c => c.Id))
                .ReverseMap();
            CreateMap<Board, UpdatePinnedBoardCommand>()
                .ForMember(d => d.BoardId, opt => opt.MapFrom(c => c.Id))
                .ReverseMap();

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

        // Gets only the first two characters of the initials.
        private string GetInitials(string name)
        {
            var initials = name.GetInitials();

            return !string.IsNullOrEmpty(initials) && initials.Length <= 2 ? initials : initials.Substring(0, 2);
        }
    }
}
