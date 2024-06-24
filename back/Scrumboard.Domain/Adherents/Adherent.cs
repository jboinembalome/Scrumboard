using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.Teams;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Adherents;

public sealed class Adherent: IEntity<int>
{
    public int Id { get; set; }
    public string IdentityId { get; set; }
    public ICollection<Board> Boards { get; set; }
    public ICollection<Activity> Activities { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Team> Teams { get; set; }
}
