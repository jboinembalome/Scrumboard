
namespace Scrumboard.Application.Adherents.Dtos;

public sealed class AdherentDto
{
    public int Id { get; set; }
    public string IdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Job { get; set; }
    public bool HasAvatar { get; set; }
}