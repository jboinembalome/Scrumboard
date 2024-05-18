using System;

namespace Scrumboard.Application.Common.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() 
        : base() { }
}