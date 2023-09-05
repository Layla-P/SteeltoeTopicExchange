using Messaging;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.Impl;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.RabbitMQ.Core;
using System.Text;
using System.Text.Json;

namespace KitchenService
{
    public class RabbitReceiver //: IHostedService
    {
        private readonly RabbitMqSettings _rabbitSettings;
        private readonly IModel _channel;
        private readonly RabbitTemplate _template;
        private readonly IHubContext<OrderHub> _orderHub;
        public RabbitReceiver(
            RabbitMqSettings rabbitSettings,
            IHubContext<OrderHub> hub,
            //IModel channel,
            RabbitTemplate template)
        {
            _rabbitSettings = rabbitSettings;
            //_channel = channel;
            _orderHub = hub;
            _template = template;
        }


        //public  Task StartAsync(CancellationToken cancellationToken)
        //{
        //   DoStuff();
        //   return Task.CompletedTask;
        //}

        //public Task StopAsync(CancellationToken cancellationToken)
        //{
        //    _channel.Dispose();
        //    return Task.CompletedTask;
        //}

        //[RabbitListener(Binding = "waffle.binding")]
        //private async Task Listen(string input)
        //{
        //    var order = JsonSerializer.Deserialize<Order>(input);
        //    await _orderHub.Clients.All.SendAsync("new-order", order);
        //}
        [RabbitListener(Binding = "waffle.binding")]
        public void DoStuff(string orderMessage)
        {
            Console.WriteLine("Success: Received Order"+ orderMessage );
            var order = JsonSerializer.Deserialize<Order>(orderMessage);
            _orderHub.Clients.All.SendAsync("new-order", order);
            //_channel.QueueBind(queue: _rabbitSettings.QueueName,
            //                  exchange: _rabbitSettings.ExchangeName,
            //                  routingKey: _rabbitSettings.RoutingKey);


            //var consumerAsync = new AsyncEventingBasicConsumer(_channel);
            ////_template.AddListener();

            //consumerAsync.Received += async (_, ea) =>
            //{
            //    var body = ea.Body.ToArray();
            //    var message = Encoding.UTF8.GetString(body);
            //    var order = JsonSerializer.Deserialize<Order>(message);
            //    await _orderHub.Clients.All.SendAsync("new-order", order);

            //    _channel.BasicAck(ea.DeliveryTag, false);
            //};

            //_channel.BasicConsume(queue: _rabbitSettings.QueueName,
            //                     autoAck: false,
            //                     consumer: consumerAsync);
        }
    }
}