using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Domain.Entities;
using System.Linq;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Dtos;
using Scrumboard.Application.ListBoards.Dtos;
using Scrumboard.Application.Teams.Dtos;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Attachments;
using Scrumboard.Domain.Cards.Checklists;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Activity, ActivityDto>()
            .ForMember(d => d.ActivityType, opt => opt.MapFrom(c => c.ActivityType.ToString()));

        CreateMap<ActivityField, ActivityFieldDto>()
            .ReverseMap();

        CreateMap<Adherent, AdherentDto>()
            .EqualityComparison((d, opt) => d.Id == opt.Id)
            .ReverseMap();

        CreateMap<IUser, AdherentDto>()
            .EqualityComparison((d, opt) => d.Id == opt.IdentityId)
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.IdentityId, opt => opt.UseDestinationValue())
            .ForMember(d => d.HasAvatar, opt => opt.MapFrom(c => c.Avatar.Any()));

        CreateMap<Board, BoardDto>()
            .ForMember(d => d.Initials, opt => opt.MapFrom(c => GetInitials(c)))
            .ForMember(d => d.LastActivity, opt => opt.MapFrom(c => c.LastModifiedDate))
            .ForMember(d => d.Members, opt => opt.MapFrom(c => c.Team.Adherents.Count));

        CreateMap<Board, BoardDetailDto>();

        CreateMap<BoardSetting, BoardSettingDto>()
            .ReverseMap();

        CreateMap<Card, CardDto>()
            .EqualityComparison((d, opt) => d.Id == opt.Id)
            .ForMember(d => d.ListBoardId, opt => opt.MapFrom(c => c.ListBoard.Id))
            .ForMember(d => d.ChecklistItemsCount, opt => opt.MapFrom(c => c.Checklists.SelectMany(ch => ch.ChecklistItems).Count()))
            .ForMember(d => d.ChecklistItemsDoneCount, opt => opt.MapFrom(c => c.Checklists.SelectMany(ch => ch.ChecklistItems).Count(i => i.IsChecked)))
            .ReverseMap();

        CreateMap<Card, CardDetailDto>()
            .EqualityComparison((d, opt) => d.Id == opt.Id)
            .ForMember(d => d.ListBoardId, opt => opt.MapFrom(c => c.ListBoard.Id))
            .ForMember(d => d.ListBoardName, opt => opt.MapFrom(c => c.ListBoard.Name))
            .ForMember(d => d.BoardId, opt => opt.MapFrom(c => c.ListBoard.Board.Id))
            .ReverseMap();

        CreateMap<Checklist, ChecklistDto>()
            .EqualityComparison((d, opt) => d.Id == opt.Id)
            .ReverseMap();

        CreateMap<ChecklistItem, ChecklistItemDto>()
            .EqualityComparison((d, opt) => d.Id == opt.Id)
            .ReverseMap();

        CreateMap<Comment, CommentDto>()
            .EqualityComparison((d, opt) => d.Id == opt.Id)
            .ReverseMap();
            
        CreateMap<Attachment, AttachmentDto>()
            .EqualityComparison((d, opt) => d.Id == opt.Id)
            .ReverseMap();

        CreateMap<Colour, ColourDto>()
            .ForMember(d => d.Colour, opt => opt.MapFrom(c => c.Code))
            .ReverseMap();

        CreateMap<Label, LabelDto>()
            .EqualityComparison((d, opt) => d.Id == opt.Id)
            .ForMember(d => d.CardIds, opt => opt.MapFrom(c => c.Cards.Select(c => c.Id)))
            .ReverseMap();

        CreateMap<ListBoard, ListBoardDto>()
            .EqualityComparison((d, opt) => d.Id == opt.Id)
            .ReverseMap();

        CreateMap<Team, TeamDto>()
            .ReverseMap();
    }

    // Gets only the first two characters of the initials.
    private static string GetInitials(Board board)
    {
        var initials = board.GetInitials();

        return !string.IsNullOrEmpty(initials) && initials.Length <= 2 
            ? initials 
            : initials.Substring(0, 2);
    }
}