using System.Collections.Generic;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Domain.Adherents;

public sealed class Adherent: IEntity<int>
{
    public int Id { get; set; }
    public string IdentityId { get; set; }
    public ICollection<Board> Boards { get; set; }
    public ICollection<Card> Cards { get; set; }
    public ICollection<Activity> Activities { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Team> Teams { get; set; }
}