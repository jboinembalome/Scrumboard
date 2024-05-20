using Scrumboard.Infrastructure.Abstractions.Common;

namespace Scrumboard.Infrastructure.Common;

internal sealed class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}