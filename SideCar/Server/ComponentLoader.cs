using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace SideCar.Server;

internal interface IComponentStrategy
{
    ITypeStrategy? GetLoader(string type);
    Task<T?> ExecuteStrategyForComponent<T>(Config config, IStrategy<T> strategy);
}

internal class ComponentStrategy : IComponentStrategy
{
    private readonly List<ITypeStrategy> _loaders;

    public ComponentStrategy(IEnumerable<ITypeStrategy> strategies)
        => _loaders = strategies.ToList();

    public ITypeStrategy? GetLoader(string type)
        => _loaders.FirstOrDefault(loader => loader.IsSupported(type));


    public async Task<T?> ExecuteStrategyForComponent<T>(Config config, IStrategy<T> strategy)
    {
        var typeStrategy = GetLoader(config.Kind);
        if (typeStrategy == null)
            return default;
        return await typeStrategy.ExecuteStrategy(config, strategy);
    }
}

internal interface ITypeStrategy
{
    string Type { get; }

    public bool IsSupported(string type)
        => string.Equals(Type, type, StringComparison.OrdinalIgnoreCase);

    Task<T?> ExecuteStrategy<T>(Config config, IStrategy<T> strategy);
}

public class PersistenceStoreStrategy : ITypeStrategy
{
    public string Type => "Persistence";
    private readonly IServiceProvider _provider;

    public PersistenceStoreStrategy(IServiceProvider provider)
        => _provider = provider;

    public async Task<T?> ExecuteStrategy<T>(Config config, IStrategy<T> strategy)
    {
        var dataType = new[] { strategy.GetType(), typeof(T) };
        var genericBase = typeof(IStrategyExecutor<,>);
        var combinedType = genericBase.MakeGenericType(dataType);
        
        var services = _provider.GetServices(combinedType);
        foreach (var service in services)
        {
            // 🤮
            const string methodName = nameof(IStrategyExecutor<IStrategy<T>, T>.Run);
            var magicMethod = combinedType.GetMethod(methodName);
            if (magicMethod == null)
                return default;

            var value = (Task<T>?)magicMethod.Invoke(service,new object[]{config, strategy});
            if (value == null)
                return default;
            
            return await value;
        }

        return default;
    }
}

public interface IStrategy<TReturn>
{
}

public class RetrieveStrategy : IStrategy<string>
{
    public string Key { get; set; }
}

public interface IStrategyExecutor<TStrategy, TReturn>
    where TStrategy : IStrategy<TReturn>
{
    string Type { get; }

    public bool IsSupported(string type)
        => string.Equals(Type, type, StringComparison.OrdinalIgnoreCase);

    Task<TReturn> Run(Config config, TStrategy strategy);
}

public class RedisStrategyExecutor
    : IStrategyExecutor<RetrieveStrategy, string>
{
    public string Type => "redis";

    public async Task<string> Run(Config config, RetrieveStrategy strategy)
    {
        var connectionString = config.GetParameter("connectionString");
        var connection = await ConnectionMultiplexer.ConnectAsync(connectionString.Value);
        var database = connection.GetDatabase();
        return await database.StringGetAsync(strategy.Key);
    }
}