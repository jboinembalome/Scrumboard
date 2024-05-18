using System;

namespace Scrumboard.Application.Common.Exceptions;

public sealed class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() 
        : base() { }
}