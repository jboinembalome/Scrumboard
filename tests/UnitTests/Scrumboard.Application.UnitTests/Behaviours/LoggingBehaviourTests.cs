﻿using Moq;
using Scrumboard.Application.Behaviours;
using Scrumboard.Application.Features.Boards.Commands.CreateBoard;
using Scrumboard.Application.Interfaces.Common;
using Scrumboard.Application.Interfaces.Identity;
using Scrumboard.Application.Interfaces.Logging;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Application.UnitTests.Behaviours
{
    public class LoggingBehaviourTests
    {
        private readonly Mock<IAppLogger<CreateBoardCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IIdentityService> _identityService;

        public LoggingBehaviourTests()
        {
            _logger = new Mock<IAppLogger<CreateBoardCommand>>();
            _currentUserService = new Mock<ICurrentUserService>();
            _identityService = new Mock<IIdentityService>();
        }

        [Fact]
        public async Task Process_CallGetUserNameAsyncOnceIfAuthenticated()
        {
            _currentUserService.Setup(x => x.UserId).Returns("43a19f1f-df2e-418f-bc4e-e0e98c792beb");

            var loggingBehaviour = new LoggingBehaviour<CreateBoardCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await loggingBehaviour.Process(new CreateBoardCommand { UserId = _currentUserService.Object.UserId }, new CancellationToken());

            _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>(), default), Times.Once);
        }

        [Fact]
        public async Task Process_NotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var requestLogger = new LoggingBehaviour<CreateBoardCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreateBoardCommand(), new CancellationToken());

            _identityService.Verify(i => i.GetUserNameAsync(null, default), Times.Never);
        }

    }
}
