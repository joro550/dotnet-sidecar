using SideCar.Server;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using SideCar.Redis;
using SideCar.Server.Configuration;
using SideCar.Server.Loaders;
using SideCar.Server.Strategies;

namespace SideCar.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper) 
        => _testOutputHelper = testOutputHelper;

    [Fact]
    public async Task Test2()
    {
        var serviceBuilder = new ServiceCollection();
        
        serviceBuilder.AddTransient<IStrategyExecutor<RetrieveStrategy,string>, RedisStrategyExecutor>();
        serviceBuilder.AddTransient<IStrategyExecutor<StoreStrategy, bool>, RedisStrategyExecutor>();

        var parameter = new Parameter
        {
            Name = "connectionString",
            Value = "localhost"
        };
        
        var persistenceType = new ComponentConfiguration
        {
            Type = "redis", Parameters = new List<Parameter> { parameter }
        };

        var config = new Config { Kind = "Persistence", Configuration = persistenceType };
        
        var strategy = new PersistenceStoreStrategy(serviceBuilder.BuildServiceProvider());

        await strategy.ExecuteStrategy(config, new StoreStrategy { Key = "name", Data = "Mark" });
        var value = await strategy.ExecuteStrategy(config, new RetrieveStrategy { Key = "name" });

        _testOutputHelper.WriteLine(value);
    }
}