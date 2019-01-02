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
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using ServiceBus.LoggingPlugin;
using ServiceBus.LoggingPlugin.Services.Storage;

namespace ServiceBusSendingToQueue
{
    internal class Program
    {
        private const string ServiceBusConnectionString = "Insert Bus ConnectionString";
        private const string QueueName = "plugin";
        private static IQueueClient queueClient;

        private static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();

            Console.ReadLine();
        }

        private static async Task MainAsync()
        {
            queueClient =
                new QueueClient(ServiceBusConnectionString, QueueName);

            //queueClient.RegisterLoggingService(new CustomLoggingService());

            queueClient.RegisterPlugin(new LoggingPlugin(configurations =>
            {
                configurations.LoggingType = LoggingType.StorageTable;
                configurations.ConfigureStorageAccount(information =>
                    {
                        information.ConnectionString = "Insert Storage Connection here";
                    });
            }));

            await SendMessagesAsync(10);
        }

        private static async Task SendMessagesAsync(int numberOfMessagesToSend = 1)
        {
            var client = new ManagementClient(ServiceBusConnectionString);

            if (!await client.QueueExistsAsync(QueueName))
                await client.CreateQueueAsync(QueueName);

            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    // Create a new message to send to the queue.
                    var messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console.
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the queue.
                    await queueClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}