using System;
using System.Collections.Generic;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.ListBoards;

public sealed class ListBoard : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
    public Board Board { get; set; }
    public ICollection<Card> Cards { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}