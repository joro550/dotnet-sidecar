using SideCar.Server.Configuration;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SideCar.Server.Loaders;

public static class ConfigLoader
{
    public static async Task<Config> LoadFromFile(string fileName)
    {
        var directory = Directory.GetCurrentDirectory();
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var readAllTextAsync = await File.ReadAllTextAsync(Path.Combine(directory, $"{fileName}.yml"));
        return deserializer.Deserialize<Config>(readAllTextAsync);
    }
}