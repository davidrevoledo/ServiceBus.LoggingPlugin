using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using ServiceBus.LoggingPlugin.Infrastructure;

namespace ServiceBus.LoggingPlugin.Abstractions
{
    /// <summary>
    ///     Service interface to log message actions
    /// </summary>
    public interface ILoggingService : IDisposableState
    { 
        /// <summary>
        ///     Log a sent message
        /// </summary>
        /// <param name="message">the sent message</param>
        /// <returns></returns>
        Task LogSentMessage(Message message);

        /// <summary>
        ///     Set Configurations to be used internally
        /// </summary>
        /// <param name="configurations"></param>
        void SetConfigurations(ILoggingConfigurations configurations);
    }
}