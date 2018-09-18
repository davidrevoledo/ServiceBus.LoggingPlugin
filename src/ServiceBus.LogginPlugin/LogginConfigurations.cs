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
using Microsoft.Azure.ServiceBus;
using ServiceBus.LogginPlugin.Abstractions;
using ServiceBus.LogginPlugin.Services.Storage;

namespace ServiceBus.LogginPlugin
{
    /// <summary>
    ///     Loggin configurations to decorate how messages are going to be logged
    /// </summary>
    public class LogginConfigurations : ILogginConfigurations
    {
        internal LogginConfigurations()
        {
            // internal ctor
        }

        /// <summary>
        ///     Is enabled then the log operation will be executed in background, but message loss is a risk
        /// </summary>
        public bool SendInBackground { get; set; }

        /// <summary>
        ///     Decoding message format
        ///     Utf8 Encoding as default
        /// </summary>
        public Func<byte[], string> Decoding { get; set; } = content => Encoding.UTF8.GetString(content);

        /// <summary>
        ///     Configuration type (Default None)
        /// </summary>
        public LogginType LogginType { get; set; } = LogginType.None;

        /// <summary>
        ///     Custom Loggin Service
        /// </summary>
        public ILogginService CustomLogginService { get; set; }

        /// <summary>
        ///     If seted up this method will be called with each sent message in adition with the provided Service
        /// </summary>
        public Action<Message> Log { get; set; }

        /// <summary>
        ///     Storage account information 
        /// </summary>
        public StorageAccountInformation StorageAccountInformation { get; set; }
    }
}