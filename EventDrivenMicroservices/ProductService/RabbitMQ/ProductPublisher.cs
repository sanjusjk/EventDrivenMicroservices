using System.Text;
using System.Text.Json;
using ProductService.Models;

namespace ProductService.RabbitMQ;

public class ProductPublisher
{
    private readonly RabbitMqConnection _rabbitMqConnection;

    public ProductPublisher(RabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    public void PublishProductUpdate(Product product)
    {
        var channel = _rabbitMqConnection.CreateChannel();
        var message = JsonSerializer.Serialize(product);
        var body = Encoding.UTF8.GetBytes(message);
        channel.QueueDeclare(queue: "ProductQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        channel.BasicPublish(
            exchange: "",
            routingKey: "ProductQueue",
            mandatory: false,
            basicProperties: null,
            body: body);
    }
}