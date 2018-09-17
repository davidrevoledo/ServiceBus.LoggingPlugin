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
using ServiceBus.LogginPlugin.Abstractions;
using ServiceBus.LogginPlugin.Services.Storage;
using ServiceBus.LogginPlugin.Services.Tracing;

namespace ServiceBus.LogginPlugin.Services
{
    /// <summary>
    ///     Service factory implementation
    /// </summary>
    internal class ServiceFactory : IServiceFactory
    {
        /// <summary>
        ///     Create service
        /// </summary>
        /// <param name="configurations">configuration to determine wich service needs to be crated</param>
        /// <returns>The Service implementation null if is None</returns>
        public ILogginService CreateService(LogginConfigurations configurations)
        {
            switch (configurations.LogginType)
            {
                case LogginType.Trace:
                    return new TraceLogginService();

                case LogginType.StorageTable:
                    return new StorageTableLogginService();

                case LogginType.Custom:
                    return CreateCustomService(configurations);

                default:
                case LogginType.None:
                    return null;
            }
        }

        /// <summary>
        ///     Create custom service
        /// </summary>
        /// <param name="configurations"></param>
        /// <returns></returns>
        private static ILogginService CreateCustomService(LogginConfigurations configurations)
        {
            if (configurations.ServiceProvider == null)
                throw new Exception("ServiceProvider should be configured to use a Custom Loggin Service");

            if (configurations.CustomLogginService == null)
                throw new Exception("CustomLogginService should be configured to indicate what service use");

            // check if the type implement ILogginService
            if (!configurations.CustomLogginService.IsAssignableFrom(typeof(ILogginService)))
                throw new Exception("CustomLogginService configured should implement ILogginService");

            return (ILogginService) configurations.ServiceProvider.GetService(configurations.CustomLogginService);
        }
    }
}