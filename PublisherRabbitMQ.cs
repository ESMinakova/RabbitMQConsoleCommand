using RabbitMQ.Client;
using System.Text;


namespace TestCase
{
    public class PublisherRabbitMQ
    {
        private readonly string hostName;
        private readonly string queueName;

        public PublisherRabbitMQ(string hostName, string queueName)
        {
            this.hostName = hostName;
            this.queueName = queueName;
        }

        public void PublishMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())            
            {
                channel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: queueName,
                    basicProperties: null,
                    body: body);

                Console.WriteLine($"Message succesfully published: {message}");
            }
        }
    }
}
