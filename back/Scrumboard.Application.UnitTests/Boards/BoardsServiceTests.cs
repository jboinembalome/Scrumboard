using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Boards;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.SharedKernel.Exceptions;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards;

public sealed class BoardsServiceTests
{
    private readonly Fixture _fixture;
    
    private readonly IMapper _mapper;
    private readonly IBoardsRepository _boardsRepository;
    private readonly IBoardsQueryRepository _boardsQueryRepository;
    private readonly IValidator<BoardCreation> _boardCreationValidator;
    private readonly IValidator<BoardEdition> _boardEditionValidator;
    private readonly ICurrentUserService _currentUserService;
    
    private readonly BoardsService _sut;

    public BoardsServiceTests()
    {
        _fixture = new CustomizedFixture();
        
        _mapper = Mock.Of<IMapper>();
        _boardsRepository = Mock.Of<IBoardsRepository>();
        _boardsQueryRepository = Mock.Of<IBoardsQueryRepository>();
        _boardCreationValidator = Mock.Of<IValidator<BoardCreation>>();
        _boardEditionValidator = Mock.Of<IValidator<BoardEdition>>();
        _currentUserService = Mock.Of<ICurrentUserService>();

        _sut = new BoardsService(
            _mapper,
            _boardsRepository,
            _boardsQueryRepository,
            _boardCreationValidator,
            _boardEditionValidator,
            _currentUserService);
    }
    
    [Fact]
    public async Task Exists_Board_should_return_true_when_Board_exists()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();
        
        Given_a_found_Board(boardId);

        // Act
        var result = await _sut.ExistsAsync(boardId);

        // Assert
        result.Should()
            .BeTrue();
    }

    [Fact]
    public async Task Exists_Board_should_return_false_when_Board_does_not_exist()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();

        Given_a_not_found_Board(boardId);
        
        // Act
        var result = await _sut.ExistsAsync(boardId);

        // Assert
        result.Should()
            .BeFalse();
    }
    
    [Fact]
    public async Task Get_Board_by_Id_should_throw_an_exception_when_Board_not_found()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();
        
        Mock.Get(_boardsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Board);

        // Act
        var act = async () => await _sut.GetByIdAsync(boardId);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Get_Board_by_Id_should_return_Board_when_found()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();
        var board = _fixture.Create<Board>();
        
        Mock.Get(_boardsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        // Act
        var result = await _sut.GetByIdAsync(boardId);

        // Assert
        result.Should()
            .Be(board);
    }
    
    [Fact]
    public async Task Get_Boards_should_return_Boards()
    {
        // Arrange
        var userId = _fixture.Create<string>();
        var boards = _fixture.CreateMany<Board>().ToList();

        Given_current_UserId(userId);

        Mock.Get(_boardsQueryRepository)
            .Setup(repo => repo.GetByOwnerIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(boards);

        // Act
        var result = await _sut.GetAsync();

        // Assert
        result.Should()
            .BeEquivalentTo(boards);
    }
    
    [Fact]
    public async Task Add_Board_should_set_OwnerId_with_current_user()
    {
        // Arrange
        var userId = _fixture.Create<string>();
        var boardCreation = _fixture.Build<BoardCreation>()
            .Without(b => b.OwnerId)
            .Create();
        var board = _fixture.Create<Board>();
        
        Given_current_UserId(userId);
        Given_mapping(boardCreation, board);
        
        // Act
        await _sut.AddAsync(boardCreation);

        // Assert
        boardCreation.OwnerId.Value.Should()
            .Be(userId);
    }

    [Fact]
    public async Task Add_Board_should_validate_BoardCreation()
    {
        // Arrange
        var boardCreation = _fixture.Build<BoardCreation>().Create();
        var board = _fixture.Create<Board>();
        
        Given_mapping(boardCreation, board);
        
        // Act
        await _sut.AddAsync(boardCreation);

        // Assert
        _boardCreationValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }
    
    [Fact]
    public async Task Add_Board_should_not_proceed_when_validation_failed()
    {
        // Arrange
        var boardCreation = _fixture.Build<BoardCreation>().Create();
        boardCreation.Name = string.Empty;
        
        _boardCreationValidator.SetupValidationFailed(
            propertyName: nameof(BoardCreation.Name), 
            errorMessage: "Name is required."
        );
        
        // Act
        var act = async () => await _sut.AddAsync(boardCreation);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
        
        Mock.Get(_boardsRepository)
            .Verify(x => x.AddAsync(
                    It.IsAny<Board>(), 
                    It.IsAny<CancellationToken>()), 
                Times.Never);
    }

    [Fact]
    public async Task Add_Board_should_map_BoardCreation_to_Board()
    {
        // Arrange
        var boardCreation = _fixture.Build<BoardCreation>().Create();
        var board = _fixture.Create<Board>();
        
        Given_mapping(boardCreation, board);

        // Act
        var result = await _sut.AddAsync(boardCreation);

        // Assert
        result.Should()
            .Be(board);
        
        Mock.Get(_mapper)
            .Verify(x => x.Map<Board>(boardCreation), Times.Once);
    }
    
    [Fact]
    public async Task Add_Board_should_call_the_repository()
    {
        // Arrange
        var boardCreation = _fixture.Build<BoardCreation>().Create();
        var board = _fixture.Create<Board>();
        
        Given_mapping(boardCreation, board);

        // Act
        await _sut.AddAsync(boardCreation);

        // Assert
        Mock.Get(_boardsRepository)
            .Verify(x => x.AddAsync(
                board, 
                It.IsAny<CancellationToken>()), 
                Times.Once);
    }
    
    [Fact]
    public async Task Update_Board_should_validate_BoardEdition()
    {
        // Arrange
        var boardEdition = _fixture.Build<BoardEdition>().Create();
        var board = Given_a_found_Board(boardEdition.Id);
        
        Given_mapping(boardEdition, board);
        
        // Act
        await _sut.UpdateAsync(boardEdition);

        // Assert
        _boardEditionValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }
    
    [Fact]
    public async Task Update_Board_should_not_proceed_when_validation_failed()
    {
        // Arrange
        var boardEdition = _fixture.Build<BoardEdition>().Create();
        boardEdition.Name = string.Empty;
        
        _boardEditionValidator.SetupValidationFailed(
            propertyName: nameof(BoardEdition.Name), 
            errorMessage: "Name is required."
        );
        
        // Act
        var act = async () => await _sut.UpdateAsync(boardEdition);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
        
        Mock.Get(_boardsRepository)
            .Verify(x => x.Update(
                    It.IsAny<Board>()), 
                Times.Never);
    }
    
    [Fact]
    public async Task Update_Board_should_not_proceed_when_Board_not_found()
    {
        // Arrange
        var boardEdition = _fixture.Build<BoardEdition>().Create();
        
        Given_a_not_found_Board(boardEdition.Id);

        // Act
        var act = async () => await _sut.UpdateAsync(boardEdition);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
        
        Mock.Get(_boardsRepository)
            .Verify(x => x.Update(
                    It.IsAny<Board>()), 
                Times.Never);
    }
    
    [Fact]
    public async Task Update_Board_should_map_BoardEdition_to_Board()
    {
        // Arrange
        var boardEdition = _fixture.Build<BoardEdition>().Create();
        var board = Given_a_found_Board(boardEdition.Id);

        Given_mapping(boardEdition, board);

        // Act
        await _sut.UpdateAsync(boardEdition);

        // Assert
        Mock.Get(_mapper)
            .Verify(x => x.Map(boardEdition, board), Times.Once);
    }
    
    [Fact]
    public async Task Update_Board_should_call_the_repository()
    {
        // Arrange
        var boardEdition = _fixture.Build<BoardEdition>().Create();
        var board = Given_a_found_Board(boardEdition.Id);

        Given_mapping(boardEdition, board);

        // Act
        await _sut.UpdateAsync(boardEdition);

        // Assert
        Mock.Get(_boardsRepository)
            .Verify(x => x.Update(board), Times.Once);
    }
    
    [Fact]
    public async Task Delete_Board_should_not_proceed_when_Board_not_found()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();
        
        Given_a_not_found_Board(boardId);

        // Act
        var act = async () => await _sut.DeleteAsync(boardId);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();

        Mock.Get(_boardsRepository)
            .Verify(x => x.DeleteAsync(
                It.IsAny<BoardId>(), 
                It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Delete_Board_should_call_the_repository()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();
        
        Given_a_found_Board(boardId);

        // Act
        await _sut.DeleteAsync(boardId);

        // Assert
        Mock.Get(_boardsRepository)
            .Verify(x => x.DeleteAsync(
                    boardId, 
                    It.IsAny<CancellationToken>()), 
                Times.Once);
    }
    
    private void Given_current_UserId(string userId) 
        => Mock.Get(_currentUserService)
            .Setup(x => x.UserId)
            .Returns(userId);
    
    private void Given_mapping(BoardCreation boardCreation, Board board) 
        => Mock.Get(_mapper)
            .Setup(x => x.Map<Board>(boardCreation))
            .Returns(board);
    
    private void Given_mapping(BoardEdition boardEdition, Board board) 
        => Mock.Get(_mapper)
            .Setup(x => x.Map<Board>(boardEdition))
            .Returns(board);

    private Board Given_a_found_Board(BoardId boardId)
    {
        var board = _fixture.Build<Board>()
            .With(x => x.Id, boardId)
            .Create();

        Mock.Get(_boardsRepository)
            .Setup(x => x.TryGetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);
        
        return board;
    }
    
    private void Given_a_not_found_Board(BoardId boardId) 
        => Mock.Get(_boardsRepository)
            .Setup(x => x.TryGetByIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Board);
}
