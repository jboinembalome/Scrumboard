using System;

namespace Scrumboard.Application.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() 
            : base() { }
    }
}
