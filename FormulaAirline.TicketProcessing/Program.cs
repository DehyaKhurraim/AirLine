// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Welcome to Ticketing Console App!");

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

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) => {
    var body = eventArgs.Body.ToArray();
    var msg = Encoding.UTF8.GetString(body);
    Console.WriteLine($"New ticket processing is initiated for - {msg}");
};

channel.BasicConsume("bookings", autoAck: true, consumer: consumer);

Console.ReadKey();
