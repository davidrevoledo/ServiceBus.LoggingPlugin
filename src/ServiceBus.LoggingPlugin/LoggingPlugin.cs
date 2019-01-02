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
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using ServiceBus.LoggingPlugin.Abstractions;
using ServiceBus.LoggingPlugin.Services;

namespace ServiceBus.LoggingPlugin
{
    public class LoggingPlugin : ServiceBusPlugin, IDisposable
    {
        private LoggingConfigurations _configurations;
        private ILoggingService _loggingService;
        private readonly IServiceFactory _serviceFactory;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="configurationsDecorator">configuration action to allow users configure</param>
        public LoggingPlugin(Action<LoggingConfigurations> configurationsDecorator)
        {
            _serviceFactory = new ServiceFactory();

            InitializeConfigurations(configurationsDecorator);
            InitializeService();
        }

        public override string Name => nameof(LoggingPlugin);

        public void Dispose()
        {
            _loggingService?.Dispose();
        }

        /// <summary>
        ///     Before send message over write
        /// </summary>
        /// <param name="message">the message before being sent</param>
        /// <returns></returns>
        public override async Task<Message> BeforeMessageSend(Message message)
        {
            if (_loggingService != null)
            {
                if (!_configurations.SendInBackground)
                    await _loggingService.LogSentMessage(message);
                else
                    // fire and forget
#pragma warning disable 4014
                    Task.Run(async () => await _loggingService.LogSentMessage(message));
#pragma warning restore 4014
            }

            _configurations.Log?.Invoke(message);

            return message;
        }

        private void InitializeService()
        {
            // finally create the client and set up configurations
            _loggingService = _serviceFactory.CreateService(_configurations);
            _loggingService?.SetConfigurations(_configurations);
        }

        /// <summary>
        ///     Init internal configurations
        /// </summary>
        /// <param name="configurationsDecorator"></param>
        private void InitializeConfigurations(Action<LoggingConfigurations> configurationsDecorator)
        {
            _configurations = new LoggingConfigurations(); // default configurations
            configurationsDecorator.Invoke(_configurations);
        }
    }
}