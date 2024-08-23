using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace FormulaAirline.API.Services
{
    public class MessageProducer : IMessageProducer
    {
        public void SendingMessage<T>(T message)
        {
            var factory = new ConnectionFactory() {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/"
            };
            var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();
            channel.QueueDeclare(
                queue: "bookings",
                durable: true,
                exclusive: false,  // Check if this should be changed to match existing declaration
                autoDelete: false,
                arguments: null
            );

            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish(exchange: "", routingKey: "bookings", basicProperties: null, body: body);
        }
    }
}