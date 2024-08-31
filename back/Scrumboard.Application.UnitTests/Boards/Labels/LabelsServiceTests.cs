using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Application.Boards.Labels;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.SharedKernel.Exceptions;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Labels;

public sealed class LabelsServiceTests
{
    private readonly IFixture _fixture;

    private readonly IMapper _mapper;
    private readonly ILabelsQueryRepository _labelsQueryRepository;
    private readonly ILabelsRepository _labelsRepository;
    private readonly IValidator<LabelCreation> _labelCreationValidator;
    private readonly IValidator<LabelEdition> _labelEditionValidator;

    private readonly LabelsService _sut;

    public LabelsServiceTests()
    {
        _fixture = new CustomizedFixture();

        _mapper = Mock.Of<IMapper>();
        _labelsQueryRepository = Mock.Of<ILabelsQueryRepository>();
        _labelsRepository = Mock.Of<ILabelsRepository>();
        _labelCreationValidator = Mock.Of<IValidator<LabelCreation>>();
        _labelEditionValidator = Mock.Of<IValidator<LabelEdition>>();

        _sut = new LabelsService(
            _mapper,
            _labelsQueryRepository,
            _labelsRepository,
            _labelCreationValidator,
            _labelEditionValidator);
    }

    [Fact]
    public async Task Get_by_BoardId_should_return_Labels()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();
        var labels = _fixture.CreateMany<Label>().ToList();

        Mock.Get(_labelsQueryRepository)
            .Setup(x => x.GetByBoardIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(labels);

        // Act
        var result = await _sut.GetByBoardIdAsync(boardId);

        // Assert
        result.Should()
            .BeEquivalentTo(labels);
    }

    [Fact]
    public async Task Get_Labels_should_return_Labels()
    {
        // Arrange
        var labelIds = _fixture.CreateMany<LabelId>().ToList();
        var labels = _fixture.CreateMany<Label>().ToList();

        Mock.Get(_labelsQueryRepository)
            .Setup(x => x.GetAsync(labelIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(labels);

        // Act
        var result = await _sut.GetAsync(labelIds);

        // Assert
        result.Should()
            .BeEquivalentTo(labels);
    }

    [Fact]
    public async Task Get_by_Id_should_throw_an_exception_when_Label_not_found()
    {
        // Arrange
        var labelId = _fixture.Create<LabelId>();

        Mock.Get(_labelsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(labelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Label);

        // Act
        var act = async () => await _sut.GetByIdAsync(labelId);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Get_by_Id_should_return_Label_when_found()
    {
        // Arrange
        var labelId = _fixture.Create<LabelId>();
        var label = _fixture.Create<Label>();

        Mock.Get(_labelsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(labelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(label);

        // Act
        var result = await _sut.GetByIdAsync(labelId);

        // Assert
        result.Should()
            .Be(label);
    }

    [Fact]
    public async Task Add_Label_should_validate_LabelCreation()
    {
        // Arrange
        var labelCreation = _fixture.Create<LabelCreation>();

        // Act
        await _sut.AddAsync(labelCreation);

        // Assert
        _labelCreationValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }

    [Fact]
    public async Task Add_Label_should_map_LabelCreation_To_Label()
    {
        // Arrange
        var labelCreation = _fixture.Create<LabelCreation>();
        var label = _fixture.Create<Label>();

        Mock.Get(_mapper)
            .Setup(x => x.Map<Label>(labelCreation))
            .Returns(label);

        // Act
        var result = await _sut.AddAsync(labelCreation);

        // Assert
        result.Should().Be(label);
        
        Mock.Get(_mapper)
            .Verify(x => x.Map<Label>(labelCreation), Times.Once);
    }

    [Fact]
    public async Task Add_Label_should_call_the_repository()
    {
        // Arrange
        var labelCreation = _fixture.Create<LabelCreation>();
        var label = _fixture.Create<Label>();

        Mock.Get(_mapper)
            .Setup(x => x.Map<Label>(labelCreation))
            .Returns(label);

        // Act
        await _sut.AddAsync(labelCreation);

        // Assert
        Mock.Get(_labelsRepository)
            .Verify(x => x.AddAsync(label, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_Label_should_validate_LabelEdition()
    {
        // Arrange
        var labelEdition = _fixture.Create<LabelEdition>();
        var label = Given_a_found_Label(labelEdition.Id);

        Mock.Get(_mapper)
            .Setup(x => x.Map(labelEdition, label))
            .Returns(label);

        // Act
        await _sut.UpdateAsync(labelEdition);

        // Assert
        _labelEditionValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }

    [Fact]
    public async Task Update_Label_should_not_proceed_when_validation_failed()
    {
        // Arrange
        var labelEdition = _fixture.Create<LabelEdition>();
        labelEdition.Name = string.Empty;

        _labelEditionValidator.SetupValidationFailed(
            propertyName: nameof(LabelEdition.Name),
            errorMessage: "Name is required."
        );

        // Act
        var act = async () => await _sut.UpdateAsync(labelEdition);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
        
        Mock.Get(_labelsRepository)
            .Verify(x => x.Update(It.IsAny<Label>()), Times.Never);
    }

    [Fact]
    public async Task Update_Label_should_not_proceed_when_Label_not_found()
    {
        // Arrange
        var labelEdition = _fixture.Create<LabelEdition>();

        Given_a_not_found_Label(labelEdition.Id);

        // Act
        var act = async () => await _sut.UpdateAsync(labelEdition);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
        
        Mock.Get(_labelsRepository)
            .Verify(x => x.Update(It.IsAny<Label>()), Times.Never);
    }

    [Fact]
    public async Task Update_Label_should_call_the_repository()
    {
        // Arrange
        var labelEdition = _fixture.Create<LabelEdition>();
        var label = Given_a_found_Label(labelEdition.Id);

        Mock.Get(_mapper)
            .Setup(x => x.Map(labelEdition, label))
            .Returns(label);

        // Act
        await _sut.UpdateAsync(labelEdition);

        // Assert
        Mock.Get(_labelsRepository)
            .Verify(x => x.Update(label), Times.Once);
    }

    [Fact]
    public async Task Delete_Label_should_not_proceed_when_Label_not_found()
    {
        // Arrange
        var labelId = _fixture.Create<LabelId>();

        Given_a_not_found_Label(labelId);

        // Act
        var act = async () => await _sut.DeleteAsync(labelId);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
        
        Mock.Get(_labelsRepository)
            .Verify(x => x.DeleteAsync(labelId, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Delete_Label_should_call_the_repository()
    {
        // Arrange
        var labelId = _fixture.Create<LabelId>();

        Given_a_found_Label(labelId);

        // Act
        await _sut.DeleteAsync(labelId);

        // Assert
        Mock.Get(_labelsRepository)
            .Verify(x => x.DeleteAsync(labelId, It.IsAny<CancellationToken>()), Times.Once);
    }

    private Label Given_a_found_Label(LabelId labelId)
    {
        var label = _fixture.Build<Label>()
            .With(x => x.Id, labelId)
            .Create();

        Mock.Get(_labelsRepository)
            .Setup(x => x.TryGetByIdAsync(labelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(label);

        return label;
    }

    private void Given_a_not_found_Label(LabelId labelId) 
        => Mock.Get(_labelsRepository)
            .Setup(x => x.TryGetByIdAsync(labelId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Label);
}
