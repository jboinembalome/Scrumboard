using AutoFixture;
using Moq;
using Scrumboard.Application.Teams.DomainEventHandlers;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Events;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.Shared.TestHelpers.Moq;
using Xunit;

namespace Scrumboard.Application.UnitTests.Teams.DomainEventHandlers;

public sealed class CreateTeamWhenBoardCreatedDomainEventHandlerTests
{
    private readonly IFixture _fixture;
    
    private readonly ITeamsRepository _teamsRepository;
    
    private readonly CreateTeamWhenBoardCreatedDomainEventHandler _sut;

    public CreateTeamWhenBoardCreatedDomainEventHandlerTests()
    {
        _fixture = new CustomizedFixture();
        
        _teamsRepository = Mock.Of<ITeamsRepository>();
        
        _sut = new CreateTeamWhenBoardCreatedDomainEventHandler(_teamsRepository);
    }

    [Fact]
    public async Task Should_create_a_Team_when_BoardCreatedDomainEvent_is_handled()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();
        var ownerId = _fixture.Create<OwnerId>();
        var domainEvent = new BoardCreatedDomainEvent(boardId, ownerId);

        // Act
        await _sut.Handle(domainEvent, CancellationToken.None);

        // Assert
        var expectedTeam = new Team(
            name: $"Team - {boardId}",
            boardId: boardId,
            memberIds: [new MemberId(ownerId.Value)]);

        Mock.Get(_teamsRepository)
            .Verify(x => x.AddAsync(
                    CustomIt.IsEquivalentTo(expectedTeam),
                    It.IsAny<CancellationToken>()), 
                Times.Once);
    }
}
