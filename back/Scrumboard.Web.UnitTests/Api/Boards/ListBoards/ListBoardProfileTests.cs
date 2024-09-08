using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.Web.Api.Boards.Labels;
using Scrumboard.Web.Api.Boards.ListBoards;
using Scrumboard.Web.Api.Cards;
using Scrumboard.Web.Api.Users;
using Xunit;

namespace Scrumboard.Web.UnitTests.Api.Boards.ListBoards;

public sealed class ListBoardProfileTests
{
    private readonly IFixture _fixture;
   
    private readonly IMapper _mapper;
    
    public ListBoardProfileTests()
    {
        _fixture = new CustomizedFixture();
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CardProfile>();
            cfg.AddProfile<ListBoardProfile>();
            cfg.SkipUserDtoMappings();
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }
    
    [Fact]
    public void Should_map_ListBoard_to_ListBoardWithCardDto()
    {
        // Arrange
        var listBoard = Given_a_ListBoard_with_Cards();

        // Act
        var listBoardWithCardDto = _mapper.Map<ListBoardWithCardDto>(listBoard);
        
        // Assert
        var expectedListBoardWithCardDto = new ListBoardWithCardDto
        {
            Id = listBoard.Id.Value,
            Name = listBoard.Name,
            Position = listBoard.Position,
            Cards = listBoard.Cards
                .Select(card => new CardDto
                {
                    Id = card.Id.Value,
                    Name = card.Name,
                    Position = card.Position,
                    ListBoardId = card.ListBoardId.Value,
                    Labels = card.Labels
                        .Select(label => new LabelDto
                        {
                            Id = label.LabelId.Value
                            // Other properties are initialized after mapping
                        })
                        .ToArray(),
                    Assignees = card.Assignees
                        .Select(assignee => new UserDto
                        {
                            Id = assignee.AssigneeId.Value
                            // Other properties are initialized after mapping
                        })
                        .ToArray(),
                    CreatedDate = card.CreatedDate,
                    LastModifiedDate = card.LastModifiedDate
                })
                .ToArray()
        };

        listBoardWithCardDto.Should()
            .BeEquivalentTo(expectedListBoardWithCardDto);
    }

    private ListBoard Given_a_ListBoard_with_Cards()
    {
        var listBoard = _fixture.Create<ListBoard>();
        listBoard.SetProperty(x => x.Name, _fixture.Create<string>());
        listBoard.SetProperty(x => x.Position, _fixture.Create<int>());
        listBoard.SetProperty(x => x.BoardId, _fixture.Create<BoardId>());
        
        var cards = Given_some_cards(listBoard);
        listBoard.SetProperty(x => x.Cards, cards);
        
        return listBoard;
    }
    
    private List<Card> Given_some_cards(ListBoard listBoard)
    {
        var labels = Given_some_labels(listBoard.BoardId);
        var cards = _fixture.CreateMany<Card>()
            .ToList();
        
        cards.ForEach(card =>
        {
            card.SetProperty(x => x.ListBoardId, listBoard.Id);
            
            var assigneeId = _fixture.Create<AssigneeId>();
            card.AddAssignees([assigneeId]);
            
            card.AddLabels(labels.Select(x => x.Id));
        });
        
        return cards;
    }
    
    private List<Label> Given_some_labels(BoardId boardId)
    {
        var labels = _fixture.CreateMany<Label>()
            .ToList();
        
        labels.ForEach(label =>
        {
            label.SetProperty(x => x.BoardId, boardId);
            label.SetProperty(x => x.Name, _fixture.Create<string>());
            label.SetProperty(x => x.Colour, _fixture.Create<Colour>());
        });
        
        return labels;
    }
}
