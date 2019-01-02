using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using ServiceBus.LoggingPlugin;
using ServiceBus.LoggingPlugin.Abstractions;

namespace ServiceBusSendingToQueue
{
    public class CustomLoggingService : ILoggingService
    {
        public void Dispose()
        {
        }

        public bool Disposed { get; set; }

        public Task LogSentMessage(Message message)
        {
            return Task.CompletedTask;
        }

        public void SetConfigurations(ILoggingConfigurations configurations)
        {
        }
    }
}