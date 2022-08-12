using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SideCar.Server;

internal static class Constants
{
    public static int PortNumber = 1983;
}

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSideCar(this IServiceCollection collection)
    {
        collection.AddHostedService(_ =>  new SideCarBackgroundService(Constants.PortNumber));

        collection.AddTransient<IComponentStrategy, ComponentStrategy>();
        collection.AddTransient<ITypeStrategy, PersistenceStoreStrategy>();

        collection.AddTransient(typeof(IStrategyExecutor<RetrieveStrategy,string>), typeof(RedisStrategyExecutor));
        
        
        return collection;
    }
}

internal class SideCarBackgroundService : BackgroundService
{
    private readonly int _portNumber;

    public SideCarBackgroundService(int portNumber) => _portNumber = portNumber;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        => await Server.StartAsync(_portNumber, stoppingToken);
}