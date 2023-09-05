using Messaging;
using Microsoft.Extensions.Options;
using OrderService;
using RabbitMQ.Client;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Core;
using System.Text;
using System.Text.Json;

public class RabbitSender
{
    private readonly RabbitTemplate _template;
    private readonly RabbitMqSettings _rabbitSettings;
    private readonly IExchange _exchange;
    public RabbitSender(
        RabbitMqSettings rabbitSettings, 
        RabbitTemplate template,
        IExchange exchange)
    {
        _template = template;
        _rabbitSettings = rabbitSettings;
        _exchange = exchange;
    }

    public void PublishMessage<T>(T entity, string key) where T : class
    {
        var message = JsonSerializer.Serialize(entity);
       // var body = Encoding.UTF8.GetBytes(message);
       
        _template.ConvertAndSend(exchange: _exchange.ExchangeName,
             routingKey: _rabbitSettings.RoutingKey,
             message: message);
        
        Console.WriteLine(" [x] Sent '{0}':'{1}'", key, message);

    }
}