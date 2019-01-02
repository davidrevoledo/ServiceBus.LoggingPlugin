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
using ServiceBus.LoggingPlugin.Abstractions;
using ServiceBus.LoggingPlugin.Services.Storage;
using ServiceBus.LoggingPlugin.Services.Tracing;

namespace ServiceBus.LoggingPlugin.Services
{
    /// <summary>
    ///     Service factory implementation
    /// </summary>
    internal class ServiceFactory : IServiceFactory
    {
        /// <summary>
        ///     Create service
        /// </summary>
        /// <param name="configurations">configuration to determine which service needs to be crated</param>
        /// <returns>The Service implementation null if is None</returns>
        public ILoggingService CreateService(LoggingConfigurations configurations)
        {
            switch (configurations.LoggingType)
            {
                case LoggingType _ when configurations.CustomLoggingService != null:

                    if (configurations.CustomLoggingService == null)
                        throw new Exception(
                            "CustomloggingService should be configured to indicate what service to use.");

                    return configurations.CustomLoggingService;

                case LoggingType.Trace:
                    return new TraceLoggingService();

                case LoggingType.StorageTable:
                    return new StorageTableLoggingService();

                default:
                case LoggingType.None:
                    return null;
            }
        }
    }
}