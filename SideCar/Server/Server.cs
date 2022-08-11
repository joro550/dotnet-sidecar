using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using SideCar.Server.Services;

namespace SideCar.Server;

internal static class Server
{
    internal static async Task StartAsync(int port, CancellationToken token)
    {
        var builder = WebApplication.CreateBuilder();

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, port, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        });
        
        builder.Services.AddGrpc();
        var app = builder.Build();

        app.MapGrpcService<LogService>();
        app.MapGrpcService<GreeterService>();
        app.MapGrpcService<PersistenceService>();

        await app.RunAsync($"http://localhost:{port}");
    }
}