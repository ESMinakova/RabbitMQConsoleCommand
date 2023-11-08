using Microsoft.Extensions.CommandLineUtils;
using TestCase;


internal class Program
{
    private static void Main(string[] args)
    {
        DBContext.Create();

        var app = new CommandLineApplication { Name = "TestCase" };
        app.HelpOption("-?|-h|--help");

        var ProducerCommand = app.Command("Producer", config =>
        {
            config.Description = "��������� ������ �� ����� � ������� RabbitMQ.  ������� dotnet run Producer --file <��� �����>";
            var fileOption = config.Option("-n|--file|<file>", "File Full Name", CommandOptionType.SingleValue);
            config.OnExecute(() =>
            {
                return Producer.ExecuteProducerCommand(fileOption);
            });
        });


        var ConsumerCommand = app.Command("Consumer", config =>
        {
            config.Description = "�������� ������ �� ����� � ������ RabbitMQ, ��������� HTTP GET ������ � ��������� ��������� � ��.  ������� dotnet run Consumer";
            config.OnExecute(() =>
            {
                return Consumer.ExecuteConsumerCommand();
            });
        });


        app.OnExecute(() =>
        {
            app.ShowHelp();
            return 0;
        });

        app.Execute(args);

    }
}