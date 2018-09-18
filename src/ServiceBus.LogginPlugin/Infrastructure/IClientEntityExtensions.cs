using Microsoft.Azure.ServiceBus;
using ServiceBus.LogginPlugin.Abstractions;

// ReSharper disable once CheckNamespace
namespace ServiceBus.LogginPlugin
{
    public static class IClientEntityExtensions
    {
        /// <summary>
        ///     Register custom Loggin Service
        /// </summary>
        /// <param name="client">Client connection to register the plugin</param>
        /// <param name="logginService">New loggin Service to user</param>
        public static void RegisterLogginService(this IClientEntity client, ILogginService logginService)
        {
            client.RegisteredPlugins.Add(new LogginPlugin(configurations =>
            {
                configurations.CustomLogginService = logginService;
            }));
        }
    }
}