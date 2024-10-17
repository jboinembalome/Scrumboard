using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.SharedKernel.Types;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards;

public abstract class CardInputBaseValidatorTestsBase<TInput> : UnitTestsBase 
    where TInput : CardInputBase
{
    protected readonly CustomizedFixture _fixture = new();

    protected readonly IListBoardsRepository _listBoardsRepository = Mock.Of<IListBoardsRepository>();
    protected readonly ILabelsRepository _labelsRepository = Mock.Of<ILabelsRepository>();
    protected readonly IIdentityService _identityService = Mock.Of<IIdentityService>();

    protected abstract IValidator<TInput> GetValidator();

    [Fact]
    public async Task Should_have_error_when_Name_is_empty()
    {
        // Arrange
        var sut = GetValidator();

        var cardInput = _fixture.Build<TInput>()
            .With(x => x.Name, string.Empty)
            .Create();

        var userIds = cardInput.AssigneeIds
           .Select(x => (UserId)x.Value)
           .ToArray();

        Given_a_found_ListBoard(cardInput.ListBoardId);

        Given_found_Users(userIds);

        Given_found_Labels(cardInput.LabelIds);

        // Act
        var result = await sut.TestValidateAsync(cardInput);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_have_error_when_Name_exceed_255_characters()
    {
        // Arrange
        var sut = GetValidator();

        var cardInput = _fixture.Build<TInput>()
            .With(x => x.Name, new string('A', 256))
            .Create();

        var userIds = cardInput.AssigneeIds
           .Select(x => (UserId)x.Value)
           .ToArray();

        Given_a_found_ListBoard(cardInput.ListBoardId);

        Given_found_Users(userIds);

        Given_found_Labels(cardInput.LabelIds);

        // Act
        var result = await sut.TestValidateAsync(cardInput);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_have_error_when_ListBoardId_not_found()
    {
        // Arrange
        var sut = GetValidator();

        var cardInput = _fixture.Create<TInput>();

        var userIds = cardInput.AssigneeIds
           .Select(x => (UserId)x.Value)
           .ToArray();

        Given_a_not_found_ListBoard(cardInput.ListBoardId);

        Given_found_Users(userIds);

        Given_found_Labels(cardInput.LabelIds);

        // Act
        var result = await sut.TestValidateAsync(cardInput);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ListBoardId);
    }

    [Fact]
    public async Task Should_have_error_when_LabelIds_not_found()
    {
        // Arrange
        var sut = GetValidator();

        var cardInput = _fixture.Create<TInput>();

        var userIds = cardInput.AssigneeIds
           .Select(x => (UserId)x.Value)
           .ToArray();

        Given_a_found_ListBoard(cardInput.ListBoardId);

        Given_found_Users(userIds);

        Given_not_found_Labels(cardInput.LabelIds);

        // Act
        var result = await sut.TestValidateAsync(cardInput);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LabelIds);
    }

    [Fact]
    public async Task Should_have_error_when_AssigneeIds_not_found()
    {
        // Arrange
        var sut = GetValidator();

        var cardInput = _fixture.Create<TInput>();

        var userIds = cardInput.AssigneeIds
           .Select(x => (UserId)x.Value)
           .ToArray();

        Given_a_found_ListBoard(cardInput.ListBoardId);

        Given_not_found_Users(userIds);

        Given_found_Labels(cardInput.LabelIds);

        // Act
        var result = await sut.TestValidateAsync(cardInput);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AssigneeIds);
    }

    protected void Given_a_found_ListBoard(ListBoardId listBoardId)
    {
        var listBoard = _fixture.Create<ListBoard>();
        listBoard.SetProperty(x => x.Id, listBoardId);
           
        Mock.Get(_listBoardsRepository)
            .Setup(x => x.TryGetByIdAsync(listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listBoard);
    }
    
    protected void Given_found_Labels(IReadOnlyCollection<LabelId> labelIds)
    {
        var labels = labelIds
            .Select(labelId =>
            {
                var label = _fixture.Create<Label>();
                label.SetProperty(x => x.Id, labelId);

                return label;
            })
            .ToArray();
        
        Mock.Get(_labelsRepository)
            .Setup(x => x.GetAsync(labelIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(labels);
    }

    protected void Given_found_Users(IReadOnlyCollection<UserId> userIds)
    {
        var users = userIds
            .Select(userId =>
            {
                var user = Mock.Of<IUser>();
                user.Id = userId.Value;
                user.FirstName = _fixture.Create<string>();
                user.LastName = _fixture.Create<string>();
                user.Job = _fixture.Create<string>();
                user.Avatar = _fixture.Create<byte[]>();

                return user;
            })
            .ToArray();
        
        Mock.Get(_identityService)
            .Setup(x => x.GetListAsync(userIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);
    }

    private void Given_a_not_found_ListBoard(ListBoardId listBoardId)
        => Mock.Get(_listBoardsRepository)
            .Setup(x => x.TryGetByIdAsync(listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as ListBoard);
    
    private void Given_not_found_Labels(IEnumerable<LabelId> labelIds) 
        => Mock.Get(_labelsRepository)
            .Setup(x => x.GetAsync(labelIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
    
    private void Given_not_found_Users(IReadOnlyCollection<UserId> userIds) 
        => Mock.Get(_identityService)
            .Setup(x => x.GetListAsync(userIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<IUser>());
}
