using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;

namespace SideCar.Client;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSideCarClient(this IServiceCollection collection)
    {
        collection.AddSingleton(ChannelFactory.CreateClient());
        collection.AddSingleton<ISideCarClient, SideCarClient>();
        
        return collection;
    }
}