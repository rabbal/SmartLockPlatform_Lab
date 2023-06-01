using MQTTnet;
using MQTTnet.Client;
using SmartLockPlatform.Mqtt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMqttClient>(_ => new MqttFactory().CreateMqttClient());

builder.Services.AddHostedService<MqttClientHostedService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();