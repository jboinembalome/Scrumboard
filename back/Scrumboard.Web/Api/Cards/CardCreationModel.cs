﻿using Scrumboard.Web.Api.Boards.Labels;
using Scrumboard.Web.Api.Users;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Cards;

public sealed class CardCreationModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int ListBoardId { get; set; }
    public IEnumerable<LabelDto> Labels { get; set; }
    public IEnumerable<UserDto> Assignees { get; set; }
}
