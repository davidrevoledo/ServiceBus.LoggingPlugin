/*
 *  MIT License
    Copyright (c) 2017 David Revoledo

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
    // Project Lead - David Revoledo davidrevoledo@d-genix.com
 */

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ServiceBus.LoggingPlugin.Abstractions;
using ServiceBus.LoggingPlugin.Infrastructure;

//https://github.com/Azure/azure-service-bus-dotnet-plugins

namespace ServiceBus.LoggingPlugin.Services.Storage
{
    /// <summary>
    ///     Build in Service to perform logging in azure storage table
    /// </summary>
    public class StorageTableLoggingService : ILoggingService
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private CloudTableClient _client;
        private int _disposeSignaled;
        private ILoggingConfigurations _loggingConfigurations;

        /// <summary>
        ///     Log a message in a storage table
        /// </summary>
        /// <param name="message">sent message</param>
        /// <returns></returns>
        public async Task LogSentMessage(Message message)
        {
            this.ThrowIfDisposed();

            await InitializeClient()
                .ConfigureAwait(false);

            var tableReference = await GetTableReference();

            var partition = _loggingConfigurations.StorageAccountInformation.PartitionKey ??
                            "messages";

            var newMessage = new MessageEntity
            {
                PartitionKey = partition,
                MessagePartitionKey = message.PartitionKey,
                MessageSessionId = message.SessionId,
                MessageId = message.MessageId,
                RowKey = Guid.NewGuid().ToString(),
                JsonMessage = message.GetJson(),
                Content = _loggingConfigurations.Decoding?.Invoke(message.Body)
            };

            var insertOperation = TableOperation.Insert(newMessage);
            await tableReference.ExecuteAsync(insertOperation);

#if DEBUG
            Trace.TraceInformation($"Table row was inserted in {tableReference.Name}");
#endif
        }

        /// <summary>
        ///     Set Configurations to be used internally
        /// </summary>
        /// <param name="configurations"></param>
        public void SetConfigurations(ILoggingConfigurations configurations)
        {
            _loggingConfigurations = configurations;

            if (_loggingConfigurations.StorageAccountInformation == null)
                throw new Exception(
                    "Storage account information is needed, please call ConfigureStorageAccount from the plugin configurations");
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposeSignaled, 1) != 0)
                return;

            _semaphore?.Dispose();
            Disposed = true;
        }

        public bool Disposed { get; set; }

        /// <summary>
        ///     Get table reference Create if not exists
        /// </summary>
        /// <returns></returns>
        private async Task<CloudTable> GetTableReference()
        {
            await _semaphore.WaitAsync()
                .ConfigureAwait(false);
            try
            {
                var tableName = _loggingConfigurations.StorageAccountInformation.TableName ??
                                $"messageslogs{DateTime.Now:MMddyyyy}";
                var tableReference = _client.GetTableReference(tableName);

                await tableReference.CreateIfNotExistsAsync()
                    .ConfigureAwait(false);

                return tableReference;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        ///     Init table client
        /// </summary>
        /// <returns></returns>
        private async Task InitializeClient()
        {
            if (_client != null)
                return;

            await _semaphore.WaitAsync()
                .ConfigureAwait(false);

            if (_client != null)
                return;

            try
            {
                if (_loggingConfigurations == null)
                    throw new Exception("Storage Configurations are null");

                var connectionString = _loggingConfigurations.StorageAccountInformation.ConnectionString;
                var account = CloudStorageAccount.Parse(connectionString);
                _client = account.CreateCloudTableClient();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}