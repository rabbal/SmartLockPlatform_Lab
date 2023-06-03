using MQTTnet;
using MQTTnet.Client;
using SmartLockPlatform.Mqtt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(
    new MqttClientOptionsBuilder()
        //.WithTcpServer("")
        .WithWebSocketServer("localhost:8083/mqtt")
        .WithCredentials("lock1", "lock1")
        //.WithClientId("clientId")
        .Build()
);

builder.Services.AddSingleton<MqttFactory>();
builder.Services.AddHostedService<MqttClientHostedService>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.Run();