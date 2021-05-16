using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Boards.Commands.DeleteBoard;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Application.Profiles;
using Scrumboard.Application.Tests.Mocks;
using Scrumboard.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Application.Tests.Features.Boards.Commands
{
    public class DeleteBoardHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAsyncRepository<Board, Guid>> _mockBoardRepository;

        public DeleteBoardHandlerTests()
        {
            _mockBoardRepository = RepositoryMocks.GetBoardRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task DeleteBoardTest_ExistingBoardId_BoardDeleted()
        {
            // Arrange
            var handler = new DeleteBoardHandler(_mapper, _mockBoardRepository.Object);
            var deleteBoardCommand = new DeleteBoardCommand { BoardId = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE") };

            // Act
            var result = await handler.Handle(deleteBoardCommand, CancellationToken.None);
            var allBoards = await _mockBoardRepository.Object.ListAllAsync(CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            allBoards.Count.Should().Be(1);
        }

        [Fact]
        public async Task DeleteBoardTest_NoExistingBoardId_ThrowsAnExceptionNotFound()
        {
            // Arrange
            var handler = new DeleteBoardHandler(_mapper, _mockBoardRepository.Object);
            var deleteBoardCommand = new DeleteBoardCommand { BoardId = Guid.Parse("C0788D2F-8003-43C1-92A4-EDC76A7C5DDE") };

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(deleteBoardCommand, CancellationToken.None));
        }
    }
}
