using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Application.ListBoards;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.SharedKernel.Exceptions;
using Xunit;

namespace Scrumboard.Application.UnitTests.ListBoards;

public sealed class ListBoardsServiceTests
{
    private readonly IFixture _fixture;
    
    private readonly IMapper _mapper;
    private readonly IListBoardsQueryRepository _listBoardsQueryRepository;
    private readonly IListBoardsRepository _listBoardsRepository;
    private readonly IValidator<ListBoardCreation> _listBoardCreationValidator;
    private readonly IValidator<ListBoardEdition> _listBoardEditionValidator;
    
    private readonly ListBoardsService _sut;

    public ListBoardsServiceTests()
    {
        _fixture = new CustomizedFixture();
        
        _mapper = Mock.Of<IMapper>();
        _listBoardsQueryRepository = Mock.Of<IListBoardsQueryRepository>();
        _listBoardsRepository = Mock.Of<IListBoardsRepository>();
        _listBoardCreationValidator = Mock.Of<IValidator<ListBoardCreation>>();
        _listBoardEditionValidator = Mock.Of<IValidator<ListBoardEdition>>();

        _sut = new ListBoardsService(
            _mapper,
            _listBoardsQueryRepository,
            _listBoardsRepository,
            _listBoardCreationValidator,
            _listBoardEditionValidator);
    }
    
    [Fact]
    public async Task Exists_should_return_true_when_ListBoard_exists()
    {
        // Arrange
        var listBoardId = _fixture.Create<ListBoardId>();
        
        Given_a_found_ListBoard(listBoardId);

        // Act
        var result = await _sut.ExistsAsync(listBoardId);

        // Assert
        result.Should()
            .BeTrue();
    }

    [Fact]
    public async Task Exists_should_return_false_when_ListBoard_does_not_exist()
    {
        // Arrange
        var listBoardId = _fixture.Create<ListBoardId>();

        Given_a_not_found_ListBoard(listBoardId);
        
        // Act
        var result = await _sut.ExistsAsync(listBoardId);

        // Assert
        result.Should()
            .BeFalse();
    }
    
    [Fact]
    public async Task Get_by_Id_should_throw_an_exception_when_ListBoard_not_found()
    {
        // Arrange
        var listBoardId = _fixture.Create<ListBoardId>();
        
        Mock.Get(_listBoardsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as ListBoard);

        // Act
        var act = async () => await _sut.GetByIdAsync(listBoardId);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Get_by_Id_should_return_ListBoard_when_found()
    {
        // Arrange
        var listBoardId = _fixture.Create<ListBoardId>();
        var listBoard = _fixture.Create<ListBoard>();
        
        Mock.Get(_listBoardsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listBoard);

        // Act
        var result = await _sut.GetByIdAsync(listBoardId);

        // Assert
        result.Should()
            .Be(listBoard);
    }
    
    [Fact]
    public async Task Get_by_BoardId_should_return_ListBoards()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();
        var listBoards = _fixture.CreateMany<ListBoard>().ToList();

        Mock.Get(_listBoardsQueryRepository)
            .Setup(repo => repo.GetByBoardIdAsync(
                boardId, 
                It.IsAny<bool?>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(listBoards);

        // Act
        var result = await _sut.GetByBoardIdAsync(boardId, includeCards: null);

        // Assert
        result.Should()
            .BeEquivalentTo(listBoards);
    }
    
    [Fact]
    public async Task Add_should_validate_ListBoardCreation()
    {
        // Arrange
        var listBoardCreation = _fixture.Create<ListBoardCreation>();
        var listBoard = _fixture.Create<ListBoard>();
        
        Given_mapping(listBoardCreation, listBoard);
        
        // Act
        await _sut.AddAsync(listBoardCreation);

        // Assert
        _listBoardCreationValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }
    
    [Fact]
    public async Task Add_should_not_proceed_when_validation_failed()
    {
        // Arrange
        var listBoardCreation = _fixture.Build<ListBoardCreation>()
            .With(x => x.Name, string.Empty)
            .Create();
        
        _listBoardCreationValidator.SetupValidationFailed(
            propertyName: nameof(ListBoardCreation.Name), 
            errorMessage: "Name is required."
        );
        
        // Act
        var act = async () => await _sut.AddAsync(listBoardCreation);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
        
        Mock.Get(_listBoardsRepository)
            .Verify(x => x.AddAsync(
                    It.IsAny<ListBoard>(), 
                    It.IsAny<CancellationToken>()), 
                Times.Never);
    }

    [Fact]
    public async Task Add_should_map_ListBoardCreation_to_ListBoard()
    {
        // Arrange
        var listBoardCreation = _fixture.Create<ListBoardCreation>();
        var listBoard = _fixture.Create<ListBoard>();
        
        Given_mapping(listBoardCreation, listBoard);

        // Act
        var result = await _sut.AddAsync(listBoardCreation);

        // Assert
        result.Should()
            .Be(listBoard);
        
        Mock.Get(_mapper)
            .Verify(x => x.Map<ListBoard>(listBoardCreation), Times.Once);
    }
    
    [Fact]
    public async Task Add_should_call_the_repository()
    {
        // Arrange
        var listBoardCreation = _fixture.Create<ListBoardCreation>();
        var listBoard = _fixture.Create<ListBoard>();
        
        Given_mapping(listBoardCreation, listBoard);

        // Act
        await _sut.AddAsync(listBoardCreation);

        // Assert
        Mock.Get(_listBoardsRepository)
            .Verify(x => x.AddAsync(
                listBoard, 
                It.IsAny<CancellationToken>()), 
                Times.Once);
    }
    
    [Fact]
    public async Task Update_should_validate_ListBoardEdition()
    {
        // Arrange
        var listBoardEdition = _fixture.Create<ListBoardEdition>();
        var listBoard = Given_a_found_ListBoard(listBoardEdition.Id);
        
        Given_mapping(listBoardEdition, listBoard);
        
        // Act
        await _sut.UpdateAsync(listBoardEdition);

        // Assert
        _listBoardEditionValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }
    
    [Fact]
    public async Task Update_should_not_proceed_when_validation_failed()
    {
        // Arrange
        var listBoardEdition = _fixture.Build<ListBoardEdition>()
            .With(x => x.Name, string.Empty)
            .Create();
        
        _listBoardEditionValidator.SetupValidationFailed(
            propertyName: nameof(ListBoardEdition.Name), 
            errorMessage: "Name is required."
        );
        
        // Act
        var act = async () => await _sut.UpdateAsync(listBoardEdition);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
        
        Mock.Get(_listBoardsRepository)
            .Verify(x => x.Update(
                    It.IsAny<ListBoard>()), 
                Times.Never);
    }
    
    [Fact]
    public async Task Update_should_not_proceed_when_ListBoard_not_found()
    {
        // Arrange
        var listBoardEdition = _fixture.Build<ListBoardEdition>().Create();
        
        Given_a_not_found_ListBoard(listBoardEdition.Id);

        // Act
        var act = async () => await _sut.UpdateAsync(listBoardEdition);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
        
        Mock.Get(_listBoardsRepository)
            .Verify(x => x.Update(
                    It.IsAny<ListBoard>()), 
                Times.Never);
    }
    
    [Fact]
    public async Task Update_should_call_the_repository()
    {
        // Arrange
        var listBoardEdition = _fixture.Build<ListBoardEdition>().Create();
        var listBoard = Given_a_found_ListBoard(listBoardEdition.Id);

        Given_mapping(listBoardEdition, listBoard);

        // Act
        await _sut.UpdateAsync(listBoardEdition);

        // Assert
        Mock.Get(_listBoardsRepository)
            .Verify(x => x.Update(listBoard), Times.Once);
    }
    
    [Fact]
    public async Task Update_should_update_the_listBoard()
    {
        // Arrange
        var listBoardEdition = _fixture.Build<ListBoardEdition>().Create();
        var listBoard = Given_a_found_ListBoard(listBoardEdition.Id);

        // Act
        await _sut.UpdateAsync(listBoardEdition);

        // Assert
        listBoard.Name.Should().Be(listBoardEdition.Name);
        listBoard.Position.Should().Be(listBoardEdition.Position);
    }
    
    [Fact]
    public async Task Delete_should_not_proceed_when_ListBoard_not_found()
    {
        // Arrange
        var listBoardId = _fixture.Create<ListBoardId>();
        
        Given_a_not_found_ListBoard(listBoardId);

        // Act
        var act = async () => await _sut.DeleteAsync(listBoardId);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();

        Mock.Get(_listBoardsRepository)
            .Verify(x => x.DeleteAsync(
                    It.IsAny<ListBoardId>(), 
                    It.IsAny<CancellationToken>()), 
                Times.Never);
    }
    
    [Fact]
    public async Task Delete_should_call_the_repository()
    {
        // Arrange
        var listBoardId = _fixture.Create<ListBoardId>();
        
        Given_a_found_ListBoard(listBoardId);

        // Act
        await _sut.DeleteAsync(listBoardId);

        // Assert
        Mock.Get(_listBoardsRepository)
            .Verify(x => x.DeleteAsync(listBoardId, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    private void Given_mapping(ListBoardCreation listBoardCreation, ListBoard listBoard) 
        => Mock.Get(_mapper)
            .Setup(x => x.Map<ListBoard>(listBoardCreation))
            .Returns(listBoard);
    
    private void Given_mapping(ListBoardEdition listBoardEdition, ListBoard listBoard) 
        => Mock.Get(_mapper)
            .Setup(x => x.Map<ListBoard>(listBoardEdition))
            .Returns(listBoard);

    private ListBoard Given_a_found_ListBoard(ListBoardId listBoardId)
    {
        var listBoard = _fixture.Build<ListBoard>()
            .With(x => x.Id, listBoardId)
            .Create();

        Mock.Get(_listBoardsRepository)
            .Setup(x => x.TryGetByIdAsync(listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listBoard);
        
        return listBoard;
    }
    
    private void Given_a_not_found_ListBoard(ListBoardId listBoardId) 
        => Mock.Get(_listBoardsRepository)
            .Setup(x => x.TryGetByIdAsync(listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as ListBoard);
}
