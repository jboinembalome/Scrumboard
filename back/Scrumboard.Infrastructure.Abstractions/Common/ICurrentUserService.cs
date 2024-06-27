
namespace Scrumboard.Infrastructure.Abstractions.Common;

public interface ICurrentUserService
{
    Guid UserId { get; }
}
