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

using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using ServiceBus.LogginPlugin.Abstractions;
using ServiceBus.LogginPlugin.Infrastructure;

namespace ServiceBus.LogginPlugin.Services.Tracing
{
    /// <summary>
    ///     Build in Service to perform trace loggin for messaging exchange
    /// </summary>
    public class TraceLogginService : ILogginService
    {
        private ILogginConfigurations _configurations;

        /// <summary>
        ///     Log a message in a Trace fashion
        ///     If a custom trace is required this class can be extended
        /// </summary>
        /// <param name="message">sent message</param>
        /// <returns></returns>
        public  virtual Task LogSentMessage(Message message)
        {
            Trace.TraceInformation("==================================");
            Trace.TraceInformation("a new message was sent");
            Trace.TraceInformation($"message sent MessageId:{message.MessageId}");
            Trace.TraceInformation($"message sent SessionId:{message.SessionId}");
            Trace.TraceInformation($"message sent PartitionKey:{message.PartitionKey}");
            Trace.TraceInformation($"message sent Body:{_configurations.Decoding?.Invoke(message.Body)}");
            Trace.TraceInformation($"message:{message.GetJson()}");

            TraceAdditionalInformation(message);

            return Task.CompletedTask;
        }

        public void SetConfigurations(ILogginConfigurations configurations)
        {
            _configurations = configurations;
        }

        public void Dispose()
        {
        }

        public bool Disposed { get; set; }

        /// <summary>
        ///     This enable the class to be exteneded for tracing purpose without having to replace completely
        /// </summary>
        /// <param name="message"></param>
        protected virtual void TraceAdditionalInformation(Message message)
        {
            // extend method
        }
    }
}