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
using ServiceBus.LogginPlugin.Abstractions;
using ServiceBus.LogginPlugin.Services;

namespace ServiceBus.LogginPlugin
{
    public class LogginPlugin : ServiceBusPlugin, IDisposable
    {
        private LogginConfigurations _configurations;
        private ILogginService _logginService;
        private IServiceFactory _serviceFactory;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="configurationsDecorator">configuration action to allow users configure</param>
        public LogginPlugin(Action<LogginConfigurations> configurationsDecorator)
        {
            InitializeConfigurations(configurationsDecorator);

            InitializeService();
        }

        public override string Name => nameof(LogginPlugin);

        public void Dispose()
        {
            _logginService?.Dispose();
        }

        /// <summary>
        ///     Before send message over write
        /// </summary>
        /// <param name="message">the message before being sent</param>
        /// <returns></returns>
        public override async Task<Message> BeforeMessageSend(Message message)
        {
            if (_logginService != null)
            {
                if (!_configurations.SendInBackground)
                    await _logginService.LogSentMessage(message);
                else
                    // fire and forget
#pragma warning disable 4014
                    Task.Run(async () => await _logginService.LogSentMessage(message));
#pragma warning restore 4014
            }

            _configurations.Log?.Invoke(message);

            return message;
        }

        private void InitializeService()
        {
            // this enable testing or the service factory can be over written 
            if (_configurations.ServiceProvider != null)
                try
                {
                    _serviceFactory =
                        (IServiceFactory)_configurations.ServiceProvider.GetService(typeof(IServiceFactory));
                }
                catch (Exception)
                {
                    // ignored
                }

            if (_serviceFactory == null)
                _serviceFactory = new ServiceFactory();

            // finally create the client and set up configurations
            _logginService = _serviceFactory.CreateService(_configurations);
            _logginService?.SetConfigurations(_configurations);
        }

        /// <summary>
        ///     Init internal configurations
        /// </summary>
        /// <param name="configurationsDecorator"></param>
        private void InitializeConfigurations(Action<LogginConfigurations> configurationsDecorator)
        {
            _configurations = new LogginConfigurations(); // default configurations
            configurationsDecorator.Invoke(_configurations);
        }
    }
}