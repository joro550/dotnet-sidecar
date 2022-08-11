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
        var parameter = new Parameter("connectionString", "localhost");
        var persistenceType = new Thing2("redis", new List<Parameter> { parameter });
        var config = new Config("Persistence", persistenceType);
        
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var yaml = serializer.Serialize(config);
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var config2 = deserializer.Deserialize<Config>(yaml); 

        _testOutputHelper.WriteLine(yaml);
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

public record Config(string Kind, Thing2 Configuration)
{
    public Config() : this(string.Empty, new Thing2())
    {
    }
}

public record Thing2(string Type, List<Parameter> Parameters)
{
    public Thing2() : this(String.Empty, new List<Parameter>())
    {
        
    }
}

public record Parameter(string Name, string Value)
{
    public Parameter() : this(string.Empty, string.Empty)
    {
        
    }
}