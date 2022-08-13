using Microsoft.Extensions.DependencyInjection;
using SideCar.Server.Configuration;
using SideCar.Server.Strategies;

namespace SideCar.Server.Loaders;

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
            const string runName = nameof(IStrategyExecutor<IStrategy<T>, T>.Run);
            const string isSupported = nameof(IStrategyExecutor<IStrategy<T>, T>.IsSupported);
            
            var runMethod = combinedType.GetMethod(runName);
            var supportedMethod = combinedType.GetMethod(isSupported);
            
            if (runMethod == null || supportedMethod == null)
                continue;

            var isTypeSupported =  (bool?)supportedMethod.Invoke(service, new object[] { config.Configuration.Type });
            if (isTypeSupported is null or false)
                continue;

            var value = (Task<T>?)runMethod.Invoke(service, new object[] { config, strategy });
            if (value == null)
                continue;
            
            return await value;
        }

        return default;
    }
}