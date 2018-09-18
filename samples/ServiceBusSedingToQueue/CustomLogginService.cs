using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using ServiceBus.LogginPlugin;
using ServiceBus.LogginPlugin.Abstractions;

namespace ServiceBusSedingToQueue
{
    public class CustomLogginService : ILogginService
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Disposed { get; set; }

        public Task LogSentMessage(Message message)
        {
            return Task.CompletedTask;
        }

        public void SetConfigurations(ILogginConfigurations configurations)
        {
        }
    }
}