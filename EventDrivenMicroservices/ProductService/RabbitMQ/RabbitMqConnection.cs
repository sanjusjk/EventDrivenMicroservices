using RabbitMQ.Client;

namespace ProductService.RabbitMQ;

public class RabbitMqConnection
{
    private readonly IConnection _connection;

    public RabbitMqConnection(IConfiguration config)
    {
        var factory = new ConnectionFactory() { HostName = config["RabbitMQ:HostName"] };
        _connection = factory.CreateConnection();
    }

    public IModel CreateChannel() => _connection.CreateModel();
}