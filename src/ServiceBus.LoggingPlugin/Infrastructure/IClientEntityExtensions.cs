using Microsoft.Azure.ServiceBus;
using ServiceBus.LoggingPlugin.Abstractions;

// ReSharper disable once CheckNamespace
namespace ServiceBus.LoggingPlugin
{
    public static class IClientEntityExtensions
    {
        /// <summary>
        ///     Register custom Logging Service
        /// </summary>
        /// <param name="client">Client connection to register the plugin</param>
        /// <param name="loggingService">New logging Service to user</param>
        public static void RegisterloggingService(this IClientEntity client, ILoggingService loggingService)
        {
            client.RegisteredPlugins.Add(new LoggingPlugin(configurations =>
            {
                configurations.CustomLoggingService = loggingService;
            }));
        }
    }
}