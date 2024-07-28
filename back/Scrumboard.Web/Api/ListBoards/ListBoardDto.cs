﻿#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.ListBoards;

public sealed class ListBoardDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
}
