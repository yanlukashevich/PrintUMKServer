using Azure.Messaging.ServiceBus;
using PrintUMKServer.Models;
using System.Text.Json;


namespace PrintUMKServer.Services.Messaging
{
    public class AzureServiceBusServie : IAzureServiceBusService
    {
        private readonly string _connectionString;
        public AzureServiceBusServie(IConfiguration configuration)
        {
            _connectionString =
                configuration["AzureServiceBus:ConnectionString"];
        }
        public async Task SendMessageAsync(string printerName, BusMessage message)
        {
            await using var client = new ServiceBusClient(_connectionString);

            // Pobieramy nazwę kolejki dla konkretnej drukarki
            string queueName = printerName.ToLower() + "-queue";
            ServiceBusSender sender = client.CreateSender(queueName);

            // Zamieniamy obiekt BusMessage na JSON
            string jsonMessage = JsonSerializer.Serialize(message);
            ServiceBusMessage busMessage = new ServiceBusMessage(jsonMessage);

            // Wysyłamy wiadomość
            await sender.SendMessageAsync(busMessage);
        }
    }
}
