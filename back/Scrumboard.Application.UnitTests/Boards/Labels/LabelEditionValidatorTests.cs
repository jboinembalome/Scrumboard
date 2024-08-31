using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Application.Boards.Labels;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Labels;

public sealed class LabelEditionValidatorTests
{
    private readonly CustomizedFixture _fixture = new();

    private readonly ILabelsRepository _labelsRepository = Mock.Of<ILabelsRepository>();
    private readonly IBoardsRepository _boardsRepository = Mock.Of<IBoardsRepository>();
    
    private readonly IValidator<LabelEdition> _sut;

    public LabelEditionValidatorTests()
    {
        _sut = new LabelEditionValidator(
            _labelsRepository, 
            _boardsRepository);
    }
    
    [Fact]
    public async Task Should_have_error_when_Name_exceed_255_characters()
    {
        // Arrange
        var labelEdition = _fixture.Build<LabelEdition>()
            .With(x => x.Name, new string('A', 256))
            .Create();
        
        // Act
        var result = await _sut.TestValidateAsync(labelEdition);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Fact]
    public async Task Should_have_error_when_BoardId_not_found()
    {
        // Arrange
        var labelEdition = _fixture.Build<LabelEdition>()
            .With(x => x.Name, new string('A', 255))
            .Create();
        
        Given_a_not_found_Board(labelEdition.BoardId);
        
        // Act
        var result = await _sut.TestValidateAsync(labelEdition);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BoardId);
    }
    
    [Fact]
    public async Task Should_have_error_when_LabelId_not_found()
    {
        // Arrange
        var labelEdition = _fixture.Build<LabelEdition>()
            .With(x => x.Name, new string('A', 255))
            .Create();
        
        Given_a_not_found_Label(labelEdition.Id);
        
        // Act
        var result = await _sut.TestValidateAsync(labelEdition);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Fact]
    public async Task Should_pass_validation()
    {
        // Arrange
        var labelEdition = _fixture.Build<LabelEdition>()
            .With(x => x.Name, new string('A', 50))
            .Create();
        
        Given_a_found_Board(labelEdition.BoardId);
        Given_a_found_Label(labelEdition.Id);
        
        // Act
        var result = await _sut.TestValidateAsync(labelEdition);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    private void Given_a_found_Board(BoardId boardId)
    {
        var board = _fixture.Build<Board>()
            .With(x => x.Id, boardId)
            .Create();

        Mock.Get(_boardsRepository)
            .Setup(x => x.TryGetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);
    }
    
    private void Given_a_not_found_Board(BoardId boardId) 
        => Mock.Get(_boardsRepository)
            .Setup(x => x.TryGetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Board);
    
    private void Given_a_found_Label(LabelId labelId)
    {
        var label = _fixture.Build<Label>()
            .With(x => x.Id, labelId)
            .Create();

        Mock.Get(_labelsRepository)
            .Setup(x => x.TryGetByIdAsync(labelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(label);
    }
    
    private void Given_a_not_found_Label(LabelId labelId) 
        => Mock.Get(_labelsRepository)
            .Setup(x => x.TryGetByIdAsync(labelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Label);
}
