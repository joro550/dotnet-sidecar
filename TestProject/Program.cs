using SideCar.Client;
using SideCar.Redis;
using SideCar.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSideCar(x => { x.AddRedis(); })
    .AddSideCarClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddSideCarLogger();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();