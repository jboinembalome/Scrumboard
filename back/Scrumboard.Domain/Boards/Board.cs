﻿using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Boards;

public sealed class Board
{
    public BoardId Id { get; set; }
    public string Name { get; set; } = "Untitled Board";
    public string Uri { get; set; } = "untitled-board";
    public bool IsPinned { get; set; }
    public BoardSetting BoardSetting { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public UserId? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}
