using Scrumboard.Application.Interfaces.Common;
using System;

namespace Scrumboard.Infrastructure.Common
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
