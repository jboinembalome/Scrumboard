namespace Scrumboard.Infrastructure.Abstractions.Identity;

public interface IUser
{
    Guid Id { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Job { get; set; }
    byte[] Avatar { get; set; }
}
