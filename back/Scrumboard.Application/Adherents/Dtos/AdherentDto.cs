#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Adherents.Dtos;

public sealed class AdherentDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Job { get; set; }
    public bool HasAvatar { get; set; }
}
