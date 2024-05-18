using System;
using Scrumboard.Infrastructure.Abstractions.Common;

namespace Scrumboard.Infrastructure.Common;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}