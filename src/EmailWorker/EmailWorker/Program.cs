using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StoreApp.Application.DTOs.EmailDtos;
using System;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "email_queue", durable: false, exclusive: false, autoDelete: false);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    var emailDto = JsonSerializer.Deserialize<EmailMessageDto>(message);

    // Email göndərmə kodu...
    Console.WriteLine($"Got email request for: {emailDto.To}");
};

channel.BasicConsume(queue: "email_queue", autoAck: true, consumer: consumer);

Console.WriteLine("Waiting for messages...");
Console.ReadLine();

