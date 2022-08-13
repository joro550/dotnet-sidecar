using SideCar.Server.Configuration;
using SideCar.Server.Strategies;

namespace SideCar.Server.Loaders;

internal interface IComponentStrategy
{
    Task<T?> ExecuteStrategyForComponent<T>(Config config, IStrategy<T> strategy);
}