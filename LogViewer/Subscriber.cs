using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogViewer;

public class Subscriber
{
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    public Subscriber(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task StartConsuming()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        string exchangeName = _configuration["RabbitMQ:Exchange"] ?? "logs";

        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout, durable: true, autoDelete: false);

        var tempQueue = await _channel.QueueDeclareAsync();
        var queueName = tempQueue.QueueName;

        await _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: string.Empty);

        Console.WriteLine($"[*] Suscrito al exchange '{exchangeName}'. Esperando logs...");

        var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>       
            {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[LOG RECIBIDO] {message}");
            await Task.Yield(); 
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
    }
}