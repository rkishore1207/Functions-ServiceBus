using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Azure.Company.Function
{
    public class MessageSender
    {
        [FunctionName("MessageSender")]
        public void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            string message = $"C# Timer trigger function executed at: {DateTime.Now}" ;
            HttpClient client = new();
            HttpRequestMessage request = new(HttpMethod.Post, "http://localhost:7071/api/MessageReceiver");
            request.Content = new StringContent(message, Encoding.UTF8,"application/json");
            client.Send(request);
            log.LogInformation("Timer Function Executed");
        }
    }
}
