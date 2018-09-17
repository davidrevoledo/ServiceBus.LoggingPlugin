using System;

namespace ServiceBus.LogginPlugin.Services.Storage
{
    public static class LogginConfigurationsExtensions
    {
        /// <summary>
        ///     Method to extend and configure storage account information
        /// </summary>
        /// <param name="configurations">plugin configurations</param>
        /// <param name="storageAccountInformationMethod">Method to complete storage account information</param>
        /// <returns>original configuration</returns>
        public static ILogginConfigurations ConfigureStorageAccount(this ILogginConfigurations configurations,
            Action<StorageAccountInformation> storageAccountInformationMethod)
        {
            var storageAccountInformation = new StorageAccountInformation();
            storageAccountInformationMethod.Invoke(storageAccountInformation);

            configurations.StorageAccountInformation = storageAccountInformation;

            return configurations;
        }
    }
}