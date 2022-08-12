using Grpc.Core;
using StackExchange.Redis;

namespace SideCar.Server.Services;

internal class PersistenceService : Persistence.PersistenceBase
{
    private readonly IDatabase _db;
    private readonly ConnectionMultiplexer _redis;
    private readonly IComponentStrategy _componentStrategy;

    public PersistenceService(IComponentStrategy componentStrategy)
    {
        _redis = ConnectionMultiplexer.Connect("localhost");
        _db = _redis.GetDatabase();

        _componentStrategy = componentStrategy;
    }
    
    public override async Task<StoreReply> StoreValue(StoreRequest request, ServerCallContext context)
    {
        var config = await ConfigLoader.LoadFromFile(request.StoreName);
        
        
        
        
        
        var success = await _db.StringSetAsync(request.Key, request.Data);
        return new StoreReply { Success = success };
    }

    public override async Task<RetrieveReply> RetrieveValue(RetrieveRequest request, ServerCallContext context)
    {
        var config = await ConfigLoader.LoadFromFile(request.StoreName);
        var loader = await _componentStrategy.ExecuteStrategyForComponent<string>(config, new RetrieveStrategy
        {
            Key = request.Key
        });
        
        
        
        
        var value = await _db.StringGetAsync(request.Key);
        return new RetrieveReply { Success = true, Data = value };
    }
}