using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Infrastructure.Persistence.Boards.Labels;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards.Labels;

public sealed class LabelsQueryRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    private readonly ILabelsQueryRepository _sut;

    public LabelsQueryRepositoryTests(DatabaseFixture databaseFixture)
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        _sut = new LabelsQueryRepository(ActDbContext);
    }

    [Fact]
    public async Task Should_get_Labels_by_BoardId()
    {
        // Arrange
        var board = await Given_a_Board();
        var labels = await Given_labels_for_board(board.Id, count: 3);

        // Act
        var result = await _sut.GetByBoardIdAsync(board.Id);

        // Assert
        result.Should()
            .BeEquivalentTo(labels);
    }

    [Fact]
    public async Task Should_get_multiple_Labels_by_Ids()
    {
        // Arrange
        var labels = await Given_labels(count: 3);
        LabelId[] labelIds = [labels[0].Id, labels[1].Id];

        // Act
        var result = await _sut.GetAsync(labelIds);

        // Assert
        result.Should()
            .BeEquivalentTo([labels[0], labels[1]]);
    }

    [Fact]
    public async Task Should_get_Label_by_Id()
    {
        // Arrange
        var label = await Given_a_Label();

        // Act
        var result = await _sut.TryGetByIdAsync(label.Id);

        // Assert
        result.Should()
            .BeEquivalentTo(label);
    }

    private async Task<Label> Given_a_Label()
    {
        var board = await Given_a_Board();
        var label = _fixture.Create<Label>();
        label.SetProperty(x => x.BoardId, board.Id);

        await ArrangeDbContext.Labels.AddAsync(label);
        await ArrangeDbContext.SaveChangesAsync();

        return label;
    }

    private async Task<List<Label>> Given_labels(int count)
    {
        var board = await Given_a_Board();
        var labels = _fixture.CreateMany<Label>(count)
            .ToList();

        labels.ForEach(label => label
            .SetProperty(x => x.BoardId, board.Id));

        await ArrangeDbContext.Labels.AddRangeAsync(labels);
        
        await ArrangeDbContext.SaveChangesAsync();

        return labels;
    }

    private async Task<List<Label>> Given_labels_for_board(BoardId boardId, int count)
    {
        var labels = _fixture.CreateMany<Label>(count)
            .ToList();
        
        labels.ForEach(label => label
            .SetProperty(x => x.BoardId, boardId));

        await ArrangeDbContext.Labels.AddRangeAsync(labels);
        
        await ArrangeDbContext.SaveChangesAsync();

        return labels;
    }

    private async Task<Board> Given_a_Board()
    {
        var board = _fixture.Create<Board>();
        
        await ArrangeDbContext.Boards.AddAsync(board);
        
        await ArrangeDbContext.SaveChangesAsync();

        return board;
    }
}
