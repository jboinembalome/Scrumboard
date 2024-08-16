﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.SharedKernel.Types;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence;

public sealed class ScrumboardDbContextTests(
    DatabaseFixture databaseFixture) : PersistenceTestsBase(databaseFixture)
{
    [Fact]
    public async void SaveChangesAsync_should_set_auditable_properties_when_creation()
    {
        // Arrange
        var userId = (UserId)Guid.NewGuid().ToString();
        SetCurrentUser(userId);

        var currentDate = DateTimeOffset.Now;
        SetCurrentDate(currentDate);
        
        var board = BuildBoard();
        
        // Act
        ActDbContext.Boards.Add(board);
        await ActDbContext.SaveChangesAsync();

        // Assert
        board.CreatedBy
            .Should()
            .Be(userId);
        
        board.CreatedDate
            .Should()
            .Be(currentDate);
    }

    [Fact]
    public async void SaveChangesAsync_should_set_auditable_properties_when_edition()
    {
        // Arrange
        var userId = (UserId)Guid.NewGuid().ToString();
        SetCurrentUser(userId);

        var currentDate = DateTimeOffset.Now;
        SetCurrentDate(currentDate);
        
        var newBoard = await Given_a_Board();
            
        // Act
        var board = await ActDbContext.Boards.FirstAsync(x => x.Id == newBoard.Id);
        board.Name = "Updated name";
        
        await ActDbContext.SaveChangesAsync();

        // Assert
        board.LastModifiedBy
            .Should()
            .Be(userId);
        
        board.LastModifiedDate
            .Should()
            .Be(currentDate);
    }

    private async Task<Board> Given_a_Board()
    {
        var board = BuildBoard();
        
        ArrangeDbContext.Boards.Add(board);
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }

    private static Board BuildBoard() 
        => new()
        {
            Name = "testBoard",
            BoardSetting = new BoardSetting
            {
                Colour = Colour.Gray
            },
            OwnerId = (OwnerId)Guid.NewGuid().ToString()
        };
}
