using Grpc.Core;

namespace SideCar.Client;

public interface ISideCarClient
{
    Task<HelloReply> GetReply(string name);
}

internal class SideCarClient : ISideCarClient
{
    private readonly ChannelBase _channel;

    public SideCarClient(ChannelBase channel) => _channel = channel;

    public async Task<HelloReply> GetReply(string name)
    {
        var greeter = new Greeter.GreeterClient(_channel);
        return await greeter.SayHelloAsync(new HelloRequest {Name = name});
    }
}