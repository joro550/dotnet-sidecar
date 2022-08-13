using SideCar.Server.Configuration;
using SideCar.Server.Strategies;

namespace SideCar.Server.Loaders;

internal interface ITypeStrategy
{
    string Type { get; }

    public bool IsSupported(string type)
        => string.Equals(Type, type, StringComparison.OrdinalIgnoreCase);

    Task<T?> ExecuteStrategy<T>(Config config, IStrategy<T> strategy);
}