using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;

namespace SmartLockPlatform.Mqtt;

public class MqttClientHostedService : BackgroundService
{
    private readonly IMqttClient _mqttClient;
    private readonly IOptions<MqttClientOptions> _options;
    private readonly IOptions<MqttClientSubscribeOptions> _subscribeOptions;

    public MqttClientHostedService(IMqttClient mqttClient,
        IOptions<MqttClientOptions> options,
        IOptions<MqttClientSubscribeOptions> subscribeOptions)
    {
        _mqttClient = mqttClient;
        _options = options;
        _subscribeOptions = subscribeOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var clientOptions = new MqttClientOptionsBuilder()
            //.WithTcpServer("")
            .WithWebSocketServer("localhost:8083/mqtt")
            .WithCredentials("lock1", "lock1")
            //.WithClientId("clientId")
            .Build();
        await _mqttClient.ConnectAsync(clientOptions, stoppingToken);

        _mqttClient.ApplicationMessageReceivedAsync +=  (e) =>
        {
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
            
            return Task.CompletedTask;
        };
        var topics = new MqttTopicFilterBuilder().WithTopic("$share/group/locks/+")
            .WithExactlyOnceQoS().Build();
        await _mqttClient.SubscribeAsync(topics, stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _mqttClient.DisconnectAsync(cancellationToken: cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}