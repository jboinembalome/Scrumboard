using Scrumboard.Infrastructure.Abstractions.Common;

namespace Scrumboard.Infrastructure.Common;

internal sealed class CurrentDateService : ICurrentDateService
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
