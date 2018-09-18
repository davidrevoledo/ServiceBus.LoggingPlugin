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
using System.Reflection;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ServiceBus.LogginPlugin.Infrastructure
{
    /// <summary>
    ///     Internal message extension methods
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        ///     Get message json body
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetJson(this Message message)
        {
            // https://github.com/Azure/azure-service-bus/issues/114 as is Server EnqueuedTimeUtc we use this to serialize the message

            //message.TimeToLive = TimeSpan.FromSeconds(10);
            var systemProperties = new Message.SystemPropertiesCollection();

            // systemProperties.EnqueuedTimeUtc = DateTime.UtcNow.AddMinutes(1);
            var bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                           BindingFlags.SetProperty;
            var value = DateTime.UtcNow.AddMinutes(1);
            systemProperties.GetType().InvokeMember("EnqueuedTimeUtc", bindings, Type.DefaultBinder, systemProperties,
                new object[] {value});
            // workaround "ThrowIfNotReceived" by setting "SequenceNumber" value
            systemProperties.GetType().InvokeMember("SequenceNumber", bindings, Type.DefaultBinder, systemProperties,
                new object[] {1});

            // message.systemProperties = systemProperties;
            bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty;
            message.GetType().InvokeMember("SystemProperties", bindings, Type.DefaultBinder, message,
                new object[] {systemProperties});

            var json = JsonConvert.SerializeObject(message);
            return json;
        }
    }
}