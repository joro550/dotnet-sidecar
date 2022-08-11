using Grpc.Net.Client;

namespace SideCar.Client;

public interface ISideCarClient
{
    Task<HelloReply> GetReply(string name);
}

internal class SideCarClient : ISideCarClient
{
    private readonly GrpcChannel _channel;

    public SideCarClient(GrpcChannel channel) => _channel = channel;

    public async Task<HelloReply> GetReply(string name)
    {
        var greeter = new Greeter.GreeterClient(_channel);
        return await greeter.SayHelloAsync(new HelloRequest {Name = name});
    }
}