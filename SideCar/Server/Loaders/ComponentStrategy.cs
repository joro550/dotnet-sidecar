using SideCar.Server.Configuration;
using SideCar.Server.Strategies;

namespace SideCar.Server.Loaders;

internal class ComponentStrategy : IComponentStrategy
{
    private readonly List<ITypeStrategy> _loaders;

    public ComponentStrategy(IEnumerable<ITypeStrategy> strategies)
        => _loaders = strategies.ToList();

    private ITypeStrategy? GetLoader(string type)
        => _loaders.FirstOrDefault(loader => loader.IsSupported(type));

    public async Task<T?> ExecuteStrategyForComponent<T>(Config config, IStrategy<T> strategy)
    {
        var typeStrategy = GetLoader(config.Kind);
        if (typeStrategy == null)
            return default;
        return await typeStrategy.ExecuteStrategy(config, strategy);
    }
}