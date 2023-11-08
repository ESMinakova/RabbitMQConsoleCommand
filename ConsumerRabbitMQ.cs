using Google.Protobuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace TestCase
{
    public class ConsumerRabbitMQ
    {
        private readonly string queueName;        
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly HttpClient client;
        private List<string> messages;

        public ConsumerRabbitMQ (string queueName, string hostName)
        {
            this.queueName = queueName;              
            var factory = new ConnectionFactory { HostName = hostName };
            connection = factory.CreateConnection ();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            client = new HttpClient () { Timeout = TimeSpan.FromMilliseconds(500) };
            messages = new List<string>();
        }

        public void Start()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ProcessMessage!;
            channel.BasicConsume(queue:  queueName,
                autoAck: true,
                consumer:  consumer);
            Console.WriteLine( "Consume started");
            
            channel.Dispose();
            connection.Dispose();            
            messages.ForEach(async str => await HttpGetRequest(str));
        }

        private async void ProcessMessage(object model, BasicDeliverEventArgs e)
        {
            try
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await Console.Out.WriteLineAsync($"message recieved {message}");
                messages.Add(message);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"{ex.Message}");
                channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: true);
            }
        }
        private async Task HttpGetRequest(string url)
        {
            var result = client.GetAsync(url).Result;
            if (result.IsSuccessStatusCode)
            {                
                var resultBody = result.Content.ReadAsStringAsync().Result;
                if (resultBody != null)
                {
                    DBContext.AddDataToDataBase(url, resultBody);
                };
            }
            else
                await Console.Out.WriteLineAsync($"Ошибка при выполнении запроса");        
        }
    }
}
