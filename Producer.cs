using Microsoft.Extensions.CommandLineUtils;

namespace TestCase
{
    public class Producer
    {
        public static int ExecuteProducerCommand(CommandOption? fileOption)
        {
            ReadFile(fileOption!);
            return 0;
        }

        private static void ReadFile(CommandOption? fileOption)
        {
            if (fileOption!.HasValue())
            {
                var hostName = "localhost";
                var queueName = "MyQueue";
                var publisher = new PublisherRabbitMQ(hostName, queueName);
                var fileInfo = new FileInfo(fileOption.Value());                
                File.ReadLines(fileInfo.FullName).ToList().ForEach(line => publisher.PublishMessage(line.Trim()));
            }
            else
                Console.WriteLine("Введите адрес файла после команды Producer --file");
        }
    }
}
