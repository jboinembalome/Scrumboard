using Scrumboard.Infrastructure.Abstractions.Common;

namespace Scrumboard.Infrastructure.Common;

internal sealed class CurrentDateServiceService : ICurrentDateService
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
