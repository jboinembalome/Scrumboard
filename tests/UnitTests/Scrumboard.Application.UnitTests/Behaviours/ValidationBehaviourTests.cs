﻿using AutoMapper;
using FluentAssertions;
using Moq;
using Scrumboard.Application.Profiles;
using Scrumboard.Application.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scrumboard.Application.Boards.Commands.UpdateBoard;
using Scrumboard.Application.Common.Behaviours;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;
using Xunit;

namespace Scrumboard.Application.UnitTests.Behaviours
{
    public class ValidationBehaviourTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAsyncRepository<Board, int>> _mockBoardRepository;

        public ValidationBehaviourTests()
        {
            _mockBoardRepository = RepositoryMocks.GetBoardRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }


        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public async Task Handle_EmptyName_ThrowsAnValidationException(string boardName)
        {
            var handler = new UpdateBoardCommandHandler(_mapper, _mockBoardRepository.Object);
            var updateBoardCommand = new UpdateBoardCommand { Name = boardName, BoardId = 1 };
             var validationBehavior = new ValidationBehaviour<UpdateBoardCommand, UpdateBoardCommandResponse>(new List<UpdateBoardCommandValidator>()
            {
                new UpdateBoardCommandValidator()
            });

            // Act
            Func<Task> action = async () =>
            {
                await validationBehavior.Handle(updateBoardCommand, new CancellationToken(), () =>
                {
                    return handler.Handle(updateBoardCommand, new CancellationToken());
                });
            };

            // Assert
            await action.Should().ThrowAsync<ValidationException>();
        }
    }
}
