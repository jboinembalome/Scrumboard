using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Infrastructure.Persistence.Boards.Labels;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Boards.Labels;

public sealed class LabelsRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    private readonly ILabelsRepository _sut;

    public LabelsRepositoryTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        
        _sut = new LabelsRepository(ActDbContext);
    }
    
    [Fact]
    public async Task Should_add_Label()
    {           
        // Arrange
        var board = await Given_a_Board();
        
        var label = _fixture.Create<Label>();
        label.SetProperty(x => x.BoardId, board.Id);
            
        // Act
        await _sut.AddAsync(label);
        await ActDbContext.SaveChangesAsync();
        
        // Assert
        var createdLabel = await AssertDbContext.Labels
            .FirstAsync(x => x.Id == label.Id);

        label.Should()
            .BeEquivalentTo(createdLabel);
    }

    [Fact]
    public async Task Should_delete_Label()
    {           
        // Arrange
        var label = await Given_a_Label();
        
        // Act
        await _sut.DeleteAsync(label.Id);
        await ActDbContext.SaveChangesAsync();
    
        // Assert
        var labelExists = await AssertDbContext.Labels
            .AnyAsync(x => x.Id == label.Id);
        
        labelExists.Should()
            .BeFalse();
    }

    [Fact]
    public async Task Should_get_Label_by_Id()
    {           
        // Arrange
        var existingLabel = await Given_a_Label();
        
        // Act
        var label = await _sut.TryGetByIdAsync(existingLabel.Id);
    
        // Assert
        label.Should()
            .BeEquivalentTo(existingLabel);
    }

    [Fact]
    public async Task Should_update_Label()
    {           
        // Arrange
        var label = await Given_a_Label_for_edition();
        label.SetProperty(x => x.Name, _fixture.Create<string>());
        label.SetProperty(x => x.Colour, _fixture.Create<Colour>());
        
        // Act
        _sut.Update(label);
        await ActDbContext.SaveChangesAsync();
        
        // Assert
        var updatedLabel = await AssertDbContext.Labels
            .FirstAsync(x => x.Id == label.Id);

        updatedLabel.Should()
            .BeEquivalentTo(label);
    }

    [Fact]
    public async Task Should_get_multiple_Labels_by_Ids()
    {
        // Arrange
        var labels = await Given_labels(3);
        LabelId[] labelIds = [labels[0].Id, labels[1].Id];

        // Act
        var result = await _sut.GetAsync(labelIds);

        // Assert
        result.Should()
            .BeEquivalentTo([labels[0], labels[1]]);
    }

    [Fact]
    public async Task GetAsync_should_return_empty_list_when_no_ids_provided()
    {
        // Arrange
        var labelIds = Enumerable.Empty<LabelId>();

        // Act
        var result = await _sut.GetAsync(labelIds);

        // Assert
        result.Should().BeEmpty();
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

    private async Task<Label> Given_a_Label_for_edition()
    {
        var label = await Given_a_Label();
        
        return await ActDbContext.Labels
            .FirstAsync(x => x.Id == label.Id);
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

    private async Task<Board> Given_a_Board()
    {
        var board = _fixture.Create<Board>();
        
        await ArrangeDbContext.Boards.AddAsync(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
}
