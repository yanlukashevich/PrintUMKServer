using PrintUMKServer.Models;

namespace PrintUMKServer.Services.Messaging
{
    public interface IAzureServiceBusService
    {
        Task SendMessageAsync(string printerName, BusMessage message);
    }
}
