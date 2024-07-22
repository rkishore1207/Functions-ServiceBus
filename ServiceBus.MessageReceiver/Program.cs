using Azure.Messaging.ServiceBus;
using System.Data;

namespace ServiceBus.MessageReceiver
{
    internal class Program
    {
        private static string? ConnectionString = "";
        private static string? TopicName = "initial-topic";
        private static string? SubscriberName = "subscriber1";
        private static async Task MessageReceiver()
        {
            ServiceBusClient client = new ServiceBusClient(ConnectionString);
            ServiceBusProcessor processor = client.CreateProcessor(TopicName,SubscriberName,new ServiceBusProcessorOptions());
            try
            {
                processor.ProcessMessageAsync += MessageHandler;
                processor.ProcessErrorAsync += ErrorHandler;

                await processor.StartProcessingAsync();
                Console.WriteLine("Press Key to Stop Processing...");
                Console.ReadKey();
                await processor.StopProcessingAsync();
                Console.WriteLine("Process Stopped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await client.DisposeAsync();
                await processor.DisposeAsync();
            }
        }

        private static async Task MessageHandler(ProcessMessageEventArgs process)
        {
            string message = process.Message.Body.ToString();
            Console.WriteLine(message);
            await process.CompleteMessageAsync(process.Message);
        }

        private static async Task ErrorHandler(ProcessErrorEventArgs error)
        {
            string message = error.Exception.Message.ToString();
            Console.WriteLine(message);
            await Task.CompletedTask;
        }

        static async Task Main(string[] args)
        {
            await MessageReceiver();
        }
    }
}
