using SideCar.Server;
using SideCar.Server.Configuration;
using SideCar.Server.Strategies;
using StackExchange.Redis;

namespace SideCar.Redis;

public class RedisStrategyExecutor
    :   IStrategyExecutor<RetrieveStrategy, string>, 
        IStrategyExecutor<StoreStrategy, bool>
{
    public string Type => "redis";
    
    public async Task<bool> Run(Config config, StoreStrategy strategy)
    {
        var connectionString = config.GetParameter("connectionString");
        var connection = await ConnectionMultiplexer.ConnectAsync(connectionString.Value);
        var database = connection.GetDatabase();
        return await database.StringSetAsync(strategy.Key, strategy.Data);
    }

    public async Task<string> Run(Config config, RetrieveStrategy strategy)
    {
        var connectionString = config.GetParameter("connectionString");
        var connection = await ConnectionMultiplexer.ConnectAsync(connectionString.Value);
        var database = connection.GetDatabase();
        return await database.StringGetAsync(strategy.Key);
    }
}