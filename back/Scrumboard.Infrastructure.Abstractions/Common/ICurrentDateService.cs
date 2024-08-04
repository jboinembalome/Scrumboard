namespace Scrumboard.Infrastructure.Abstractions.Common;

public interface ICurrentDateService
{
    DateTimeOffset Now { get; }
}
