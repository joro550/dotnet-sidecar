using SideCar.Server.Strategies;
using SideCar.Server.Configuration;

namespace SideCar.Server;

public interface IStrategyExecutor<in TStrategy, TReturn>
    where TStrategy : IStrategy<TReturn>
{
    string Type { get; }

    public bool IsSupported(string type)
        => string.Equals(Type, type, StringComparison.OrdinalIgnoreCase);

    Task<TReturn> Run(Config config, TStrategy strategy);
}