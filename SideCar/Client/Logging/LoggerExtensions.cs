using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SideCar.Client;

public static class LoggerExtensions
{
    public static ILoggingBuilder AddSideCarLogger(
        this ILoggingBuilder builder)
    {

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, SideCarLoggerProvider>());
        return builder;
    }
}