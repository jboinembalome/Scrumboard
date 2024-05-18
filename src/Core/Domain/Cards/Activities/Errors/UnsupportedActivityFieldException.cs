using System;

namespace Scrumboard.Domain.Cards.Activities.Errors;

public sealed class UnsupportedActivityFieldException : Exception
{
    public UnsupportedActivityFieldException(string field)
        : base($"Activity field \"{field}\" is unsupported.")
    {
    }
}