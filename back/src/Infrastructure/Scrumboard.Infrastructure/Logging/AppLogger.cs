﻿using Microsoft.Extensions.Logging;
using Scrumboard.Infrastructure.Abstractions.Logging;

namespace Scrumboard.Infrastructure.Logging;

internal sealed class LoggerAdapter<T>(ILoggerFactory loggerFactory) : IAppLogger<T>
{
    private readonly ILogger<T> _logger = loggerFactory.CreateLogger<T>();

    public void LogWarning(string message, params object[] args) 
        => _logger.LogWarning(message, args);

    public void LogInformation(string message, params object[] args) 
        => _logger.LogInformation(message, args);

    public void LogTrace(string message, params object[] args) 
        => _logger.LogTrace(message, args);

    public void LogDebug(string message, params object[] args) 
        => _logger.LogDebug(message, args);

    public void LogError(Exception exception, string message, params object[] args) 
        => _logger.LogError(exception, message, args);

    public void LogError(string message, params object[] args) 
        => _logger.LogError(message, args);

    public void LogCritical(string message, params object[] args) 
        => _logger.LogCritical(message, args);
}
