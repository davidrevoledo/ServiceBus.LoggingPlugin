using System;

namespace ServiceBus.LoggingPlugin.Services.Storage
{
    public static class LoggingConfigurationsExtensions
    {
        /// <summary>
        ///     Method to extend and configure storage account information
        /// </summary>
        /// <param name="configurations">plugin configurations</param>
        /// <param name="storageAccountInformationMethod">Method to complete storage account information</param>
        /// <returns>original configuration</returns>
        public static ILoggingConfigurations ConfigureStorageAccount(this ILoggingConfigurations configurations,
            Action<StorageAccountInformation> storageAccountInformationMethod)
        {
            var storageAccountInformation = new StorageAccountInformation();
            storageAccountInformationMethod.Invoke(storageAccountInformation);

            configurations.StorageAccountInformation = storageAccountInformation;

            return configurations;
        }
    }
}