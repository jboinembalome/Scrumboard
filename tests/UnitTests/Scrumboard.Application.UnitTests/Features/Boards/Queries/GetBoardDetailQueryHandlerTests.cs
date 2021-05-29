using AutoMapper;
using FluentAssertions;
using Moq;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Boards.Queries.GetBoardDetail;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Application.Profiles;
using Scrumboard.Application.UnitTests.Mocks;
using Scrumboard.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Application.UnitTests.Features.Boards.Queries
{
    public class GetBoardDetailQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAsyncRepository<Board, int>> _mockBoardRepository;

        public GetBoardDetailQueryHandlerTests()
        {
            _mockBoardRepository = RepositoryMocks.GetBoardRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task GetBoardDetailTest_ExistingBoardId_ReturnsDetailsOfBoard()
        {
            // Arrange
            var handler = new GetBoardDetailQueryHandler(_mapper, _mockBoardRepository.Object);
            var getBoardDetailQuery = new GetBoardDetailQuery { BoardId = 1 };

            // Act
            var result = await handler.Handle(getBoardDetailQuery, CancellationToken.None);

            // Assert
            result.Should().BeOfType<BoardDetailDto>();
            result.Name.Should().Be("Scrumboard FrontEnd");
            result.Adherent.IdentityGuid.Should().Be("2cd08f87-33a6-4cbc-a0de-71d428986b85");
            result.Labels.First().Name.Should().Be("Design");
            result.Labels.Last().Name.Should().Be("Feature");
            result.ListBoards.First().Name.Should().Be("Design");
            result.ListBoards.First().Cards.First().Name.Should().Be("Create login page");
            result.ListBoards.First().Cards.First().Labels.First().Name.Should().Be("Design");
            result.ListBoards.First().Cards.First().Adherents.First().IdentityGuid.Should().Be("2cd08f87-33a6-4cbc-a0de-71d428986b85");
            result.ListBoards.First().Cards.First().AttachmentsCount.Should().Be(2);
            result.ListBoards.First().Cards.First().ChecklistItemsCount.Should().Be(2);
            result.ListBoards.First().Cards.First().ChecklistItemsDoneCount.Should().Be(1);
            result.ListBoards.First().Cards.First().CommentsCount.Should().Be(1);
        }

        [Fact]
        public async Task GetBoardDetailTest_NoExistingBoardId_ThrowsAnExceptionNotFound()
        {
            // Arrange
            var handler = new GetBoardDetailQueryHandler(_mapper, _mockBoardRepository.Object);
            var getBoardDetailQuery = new GetBoardDetailQuery { BoardId = 0 };

            // Act
            Func<Task> action = async () => { await handler.Handle(getBoardDetailQuery, CancellationToken.None); };

            // Assert
            await action.Should().ThrowAsync<NotFoundException>();
        }

    }
}
