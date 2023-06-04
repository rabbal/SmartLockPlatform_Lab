using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;

namespace SmartLockPlatform.Mqtt;

public class MqttClientHostedService : BackgroundService
{
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _options;
    private readonly IOptions<MqttClientSubscribeOptions> _subscribeOptions;

    public MqttClientHostedService(
        MqttFactory factory,
        MqttClientOptions options,
        IOptions<MqttClientSubscribeOptions> subscribeOptions)
    {
        _mqttClient = factory.CreateMqttClient();
        _options = options;
        _subscribeOptions = subscribeOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _mqttClient.ConnectAsync(_options, stoppingToken);

        _mqttClient.ApplicationMessageReceivedAsync +=  (e) =>
        {
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());
            
            return Task.CompletedTask;
        };
        var topics = new MqttTopicFilterBuilder()
            .WithTopic("$share/group/1")
            .WithAtLeastOnceQoS()
            .Build();
        
        await _mqttClient.SubscribeAsync(topics, stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _mqttClient.DisconnectAsync(cancellationToken: cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}