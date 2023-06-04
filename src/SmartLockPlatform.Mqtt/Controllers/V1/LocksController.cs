using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace SmartLockPlatform.Mqtt.Controllers.V1;

[ApiController, Route("api/v1.0")]
public class LocksController : ControllerBase
{
    private readonly MqttClientOptions _options;
    private readonly IMqttClient _client;

    public LocksController(MqttFactory factory, MqttClientOptions options)
    {
        _options = options;
        _client = factory.CreateMqttClient();
    }

    [HttpPost("unlock")]
    public async Task<IActionResult> UnLock([FromBody] UnLockDTO model, CancellationToken cancellationToken)
    {
        var topic = $"sites/{model.SiteId}/locks/{model.LockUuid}/open";
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            //TODO: Use at least once and handle idempotency by providing timestamp and ignore earlier ones on smart client.
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
            //Feature CorrelationData requires MQTT version 5.0.0
            //.WithResponseTopic($"$share/group/acks/{model.LockUuid}")
            //.WithCorrelationData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model)))
            .WithPayload(JsonSerializer.Serialize(model))
            .Build();

        await _client.ConnectAsync(_options, cancellationToken);
        await _client.PublishAsync(message, cancellationToken);

        await _client.DisconnectAsync(cancellationToken: cancellationToken);

        //TODO: think about persisting command beforehand

        return Ok();
    }
}

public class UnLockDTO
{
    public long SiteId { get; init; }
    public long LockId { get; init; }
    public string LockUuid { get; init; } = default!;

    public long UserId { get; init; }

    //prevent duplicate lock opening by SmartLockPlatform.Mqtt
    public string IdempotentId { get; init; } = default!;
}