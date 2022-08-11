using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace SideCar.Client;

public class SideCarLogger : ILogger, IDisposable
{
    private Logger.LoggerClient _client;

    public SideCarLogger(ChannelBase channel) 
        => _client = new Logger.LoggerClient(channel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        _client.LogInformation(new Log { Title = "Thing" });
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) => this;

    public void Dispose() { }
}

public sealed class SideCarLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, SideCarLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

    public SideCarLoggerProvider()
    {
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new SideCarLogger(ChannelFactory.CreateClient()));

    public void Dispose() => _loggers.Clear();
}