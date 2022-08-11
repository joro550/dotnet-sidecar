using Grpc.Core;
using System.Text.Json;
using Grpc.Net.Client;

namespace SideCar.Client;

public interface IStorageClient
{
    Task<StoreReply> StoreAsync<T>(string storeName, string key, T value);
    Task<T> RetrieveAsync<T>(string storeName, string key);
}

internal class StorageClient : IStorageClient
{
    private readonly Persistence.PersistenceClient _client;

    public StorageClient(GrpcChannel channel)
    {
        _client = new Persistence.PersistenceClient(channel);
    }

    public async Task<StoreReply> StoreAsync<T>(string storeName, string key, T value) 
        => await _client.StoreValueAsync(new StoreRequest {StoreName = storeName, Key = key, Data = JsonSerializer.Serialize(value)});

    public async Task<T> RetrieveAsync<T>(string storeName, string key)
    {
        var retrieveValueAsync = await _client.RetrieveValueAsync(new RetrieveRequest { StoreName = storeName, Key = key });
        return JsonSerializer.Deserialize<T>(retrieveValueAsync.Data)!;
    }
}