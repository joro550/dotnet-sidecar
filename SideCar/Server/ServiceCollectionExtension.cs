using SideCar.Server.Loaders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SideCar.Server;

internal static class Constants
{
    public const int PortNumber = 1983;
}

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSideCar(this IServiceCollection collection)
    {
        collection.AddHostedService(_ =>  new SideCarBackgroundService(Constants.PortNumber));
        collection.AddTransient<IComponentStrategy, ComponentStrategy>();
        collection.AddTransient<ITypeStrategy, PersistenceStoreStrategy>();
        return collection;
    }
    
    public static IServiceCollection AddSideCar(this IServiceCollection collection, Action<SideCarProviders> providers)
    {
        collection.AddHostedService(_ =>  new SideCarBackgroundService(Constants.PortNumber));

        collection.AddTransient<IComponentStrategy, ComponentStrategy>();
        collection.AddTransient<ITypeStrategy, PersistenceStoreStrategy>();
        
        providers(new SideCarProviders(collection));
        
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