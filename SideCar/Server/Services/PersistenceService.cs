using Grpc.Core;
using SideCar.Server.Loaders;
using SideCar.Server.Strategies;

namespace SideCar.Server.Services;

internal class PersistenceService : Persistence.PersistenceBase
{
    private readonly IComponentStrategy _componentStrategy;

    public PersistenceService(IComponentStrategy componentStrategy) 
        => _componentStrategy = componentStrategy;

    public override async Task<StoreReply> StoreValue(StoreRequest request, ServerCallContext context)
    {
        var config = await ConfigLoader.LoadFromFile(request.StoreName);
        var success = await _componentStrategy.ExecuteStrategyForComponent(config, new StoreStrategy
        {
            Key = request.Key,
            Data = request.Data
        });
        return new StoreReply { Success = success };
    }

    public override async Task<RetrieveReply> RetrieveValue(RetrieveRequest request, ServerCallContext context)
    {
        var config = await ConfigLoader.LoadFromFile(request.StoreName);
        var loader = await _componentStrategy.ExecuteStrategyForComponent(config, new RetrieveStrategy
        {
            Key = request.Key
        });
        return new RetrieveReply { Success = true, Data = loader };
    }
}