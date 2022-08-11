using Grpc.Core;
using StackExchange.Redis;

namespace SideCar.Server.Services;

public class PersistenceService : Persistence.PersistenceBase
{
    private readonly IDatabase _db;
    private ComponentLoader _componentLoader;
    private readonly ConnectionMultiplexer _redis;

    public PersistenceService()
    {
        _redis = ConnectionMultiplexer.Connect("localhost");
        _db = _redis.GetDatabase();

        _componentLoader = new ComponentLoader();
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
        var loader = _componentLoader.GetLoader(config.Kind);
        
        
        
        
        var value = await _db.StringGetAsync(request.Key);
        return new RetrieveReply { Success = true, Data = value };
    }
}