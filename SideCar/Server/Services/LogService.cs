using Grpc.Core;

namespace SideCar.Server.Services;

internal class LogService : Logger.LoggerBase
{
    public override Task<LogReturn> LogInformation(Log request, ServerCallContext context)
    {
        return Task.FromResult(new LogReturn());
    }
}