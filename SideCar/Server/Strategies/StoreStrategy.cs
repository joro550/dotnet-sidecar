namespace SideCar.Server.Strategies;

public class StoreStrategy : IStrategy<bool>
{
    public string Key { get; set; }
    public string Data { get; set; }
}