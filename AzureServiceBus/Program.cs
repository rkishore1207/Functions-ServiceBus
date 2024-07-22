using Azure.Messaging.ServiceBus;

namespace AzureServiceBus
{
    internal class Program
    {
        private static string? ServiceBusConnectionString = Environment.GetEnvironmentVariable("SERVICEBUS_CONNECTION_STRING");
        private static string? QueueName = "message-queue";
        private static int maxNumberOfMessages = 4;
        public static async Task ServiceBus()
        {
            ServiceBusClient client = new ServiceBusClient(ServiceBusConnectionString);
            ServiceBusSender sender = client.CreateSender(QueueName);

            using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();
            for(int i = 1; i <= maxNumberOfMessages; i++)
            {
                if(!batch.TryAddMessage(new ServiceBusMessage($"Message - {i}")))
                {
                    Console.WriteLine($"Mess age - {i} Not inserted into Batch");
                }
            }
            try
            {
                await sender.SendMessagesAsync(batch);
                Console.WriteLine("Message Sent");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed - {ex.Message}");
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
        static async Task Main(string[] args)
        {
            await ServiceBus();
        }
    }
}
