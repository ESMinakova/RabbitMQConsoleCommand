namespace TestCase
{
    public class Consumer
    {
        public static int ExecuteConsumerCommand()
        {
            var consumer = new ConsumerRabbitMQ("MyQueue", "localhost");
            consumer.Start();
            return 0;
        }
    }
}
