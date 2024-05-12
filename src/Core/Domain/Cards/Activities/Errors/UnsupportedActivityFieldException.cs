using System;

namespace Scrumboard.Domain.Cards.Activities.Errors;

public class UnsupportedActivityFieldException : Exception
{
    public UnsupportedActivityFieldException(string field)
        : base($"Activity field \"{field}\" is unsupported.")
    {
    }
}