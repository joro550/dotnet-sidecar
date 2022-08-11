using Microsoft.AspNetCore.Mvc;
using SideCar.Client;

namespace TestProject.Controllers;

[ApiController]
[Route("greeter")]
public class GreeterController : ControllerBase
{
    private readonly ISideCarClient _client;
    private readonly IStorageClient _storageClient;
    private readonly ILogger<GreeterController> _logger;

    public GreeterController(ISideCarClient client, ILogger<GreeterController> logger, IStorageClient storageClient)
    {
        _client = client;
        _logger = logger;
        _storageClient = storageClient;
    }

    [HttpGet]
    public async Task<string> Index()
    {
        var person = await _storageClient.RetrieveAsync<Person>("cache", "hello");
        
        
        var helloReply = await _client.GetReply("Mark");
        _logger.LogInformation("Hello");
        
        
        
        return helloReply.Message;
    }
}

public record Person (string Name);