﻿using System;

namespace Scrumboard.Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException()
        : base() { }

    public NotFoundException(string message)
        : base(message) { }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException) { }

    public NotFoundException(string name, object key)
        : base($"{name} ({key}) is not found.") { }
}