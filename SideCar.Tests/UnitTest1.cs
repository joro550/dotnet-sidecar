using Microsoft.Extensions.DependencyInjection;
using SideCar.Server;
using Xunit.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SideCar.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test1()
    {
        // var parameter = new Parameter("connectionString", "localhost");
        // var persistenceType = new ComponentConfiguration("redis", new List<Parameter> { parameter });
        // var config = new Config("Persistence", persistenceType);
        //
        // var serializer = new SerializerBuilder()
        //     .WithNamingConvention(CamelCaseNamingConvention.Instance)
        //     .Build();
        // var yaml = serializer.Serialize(config);
        //
        // var deserializer = new DeserializerBuilder()
        //     .WithNamingConvention(CamelCaseNamingConvention.Instance)
        //     .Build();
        // var config2 = deserializer.Deserialize<Config>(yaml); 
        //
        // _testOutputHelper.WriteLine(yaml);
    }

    [Fact]
    public async Task Test2()
    {
        var serviceBuilder = new ServiceCollection();
        serviceBuilder.AddTransient(typeof(IStrategyExecutor<RetrieveStrategy,string>), typeof(RedisStrategyExecutor));

        var parameter = new Parameter()
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

        await strategy.ExecuteStrategy(config, new RetrieveStrategy { Key = "name" });
    }
}

public static class ConfigLoader
{
    public static async Task<Config> LoadFromFile(string fileName)
    {
        var directory = Directory.GetCurrentDirectory();
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        try
        {
            var readAllTextAsync = await File.ReadAllTextAsync(Path.Combine(directory, $"{fileName}.yml"));
            return deserializer.Deserialize<Config>(readAllTextAsync);
        }
        catch (Exception e)
        {
            throw;
        }
        
    }
}