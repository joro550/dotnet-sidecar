using Grpc.Net.Client;

namespace SideCar.Client;

internal static class ChannelFactory
{
    public static GrpcChannel CreateClient() 
        => GrpcChannel.ForAddress($"http://localhost:{Constants.PortNumber}");
}


