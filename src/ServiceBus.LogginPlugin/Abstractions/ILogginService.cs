using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using ServiceBus.LogginPlugin.Infraestructure;

namespace ServiceBus.LogginPlugin.Abstractions
{
    /// <summary>
    ///     Service interface to logg message actions
    /// </summary>
    public interface ILogginService : IDisposableState
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
        void SetConfigurations(ILogginConfigurations configurations);
    }
}