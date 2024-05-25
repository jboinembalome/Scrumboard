namespace Scrumboard.Application.Common.Exceptions;

public sealed class AlreadyExistsException : Exception
{
    public AlreadyExistsException()
    { }

    public AlreadyExistsException(string message) 
        : base(message) { }

    public AlreadyExistsException(string message, Exception innerException)
        : base(message, innerException) { }

    public AlreadyExistsException(string name, object key)
        : base($"{name} ({key}) already exist.") { }
}
