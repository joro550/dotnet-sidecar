using SideCar.Server.Loaders;

namespace SideCar.Server.Configuration;

public class Config
{
    public string Kind { get; set; }
    public ComponentConfiguration Configuration { get; set; }

    public Parameter GetParameter(string name) 
        => Configuration.Parameters.FirstOrDefault(x => x.Name == name)!;
}