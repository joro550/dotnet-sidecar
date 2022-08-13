namespace SideCar.Server.Configuration;

public record ComponentConfiguration
{
    public string Type { get; set; } 
    public List<Parameter> Parameters { get; set; }
}