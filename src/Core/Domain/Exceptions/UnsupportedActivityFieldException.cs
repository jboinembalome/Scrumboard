using System;

namespace Scrumboard.Domain.Exceptions
{
    public class UnsupportedActivityFieldException : Exception
    {
        public UnsupportedActivityFieldException(string field)
            : base($"Acitivity field \"{field}\" is unsupported.")
        {
        }
    }
}
