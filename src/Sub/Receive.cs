using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class Send
{
    public static void Main()
    {
        var factory = new ConnectionFactory() 
        {
            HostName = "localhost",
            UserName = "admin",
            Password = "admin",
            Port = 5672,
            Ssl = new SslOption() { Enabled = false }
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);
            
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };

            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}