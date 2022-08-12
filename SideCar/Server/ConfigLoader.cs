using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SideCar.Server;

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

public class Config
{
    public string Kind { get; set; }
    public ComponentConfiguration Configuration { get; set; }

    public Parameter GetParameter(string name) 
        => Configuration.Parameters.FirstOrDefault(x => x.Name == name)!;
}

public record ComponentConfiguration
{
    public string Type { get; set; } 
    public List<Parameter> Parameters { get; set; }
}

public record Parameter
{
    public string Name { get; set; }
    public string Value { get; set; }
}