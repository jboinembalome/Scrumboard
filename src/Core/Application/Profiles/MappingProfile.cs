using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Extensions;
using Scrumboard.Application.Features.Boards.Commands.CreateBoard;
using Scrumboard.Application.Features.Boards.Commands.UpdateBoard;
using Scrumboard.Application.Features.Boards.Commands.UpdatePinnedBoard;
using Scrumboard.Application.Features.Cards.Commands.CreateCard;
using Scrumboard.Application.Features.Cards.Commands.UpdateCard;
using Scrumboard.Application.Features.Comments.Commands.CreateComment;
using Scrumboard.Application.Features.Comments.Commands.UpdateComment;
using Scrumboard.Application.Features.Teams.Commands.UpdateTeam;
using Scrumboard.Application.Interfaces.Identity;
using Scrumboard.Domain.Entities;
using Scrumboard.Domain.ValueObjects;
using System.Linq;

namespace Scrumboard.Application.Profiles
{
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
                .ForMember(d => d.Initials, opt => opt.MapFrom(c => GetInitials(c.Name)))
                .ForMember(d => d.LastActivity, opt => opt.MapFrom(c => c.LastModifiedDate))
                .ForMember(d => d.Members, opt => opt.MapFrom(c => c.Team.Adherents.Count));
            CreateMap<Board, CreateBoardCommand>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(c => c.Adherent.IdentityId))
                .ReverseMap();
            CreateMap<Board, UpdateBoardCommand>()
                .ForMember(d => d.BoardId, opt => opt.MapFrom(c => c.Id))
                .ReverseMap();
            CreateMap<Board, UpdatePinnedBoardCommand>()
                .ForMember(d => d.BoardId, opt => opt.MapFrom(c => c.Id))
                .ReverseMap();

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
      
            CreateMap<Card, CreateCardCommand>()
                .ReverseMap();

            CreateMap<Card, UpdateCardCommand>()
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

            CreateMap<Comment, CreateCommentCommand>()
               .ForMember(d => d.CardId, opt => opt.MapFrom(c => c.Card.Id))
               .ReverseMap();
            CreateMap<Comment, UpdateCommentCommand>()
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

            CreateMap<Team, UpdateTeamCommand>()
                .ReverseMap();
        }

        // Gets only the first two characters of the initials.
        private string GetInitials(string name)
        {
            var initials = name.GetInitials();

            return !string.IsNullOrEmpty(initials) && initials.Length <= 2 ? initials : initials.Substring(0, 2);
        }
    }
}
