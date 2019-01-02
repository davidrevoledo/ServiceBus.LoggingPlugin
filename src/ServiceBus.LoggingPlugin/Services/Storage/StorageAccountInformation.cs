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
namespace ServiceBus.LoggingPlugin.Services.Storage
{
    /// <summary>
    ///     Storage configurations 
    /// </summary>
    public class StorageAccountInformation 
    {
        internal StorageAccountInformation()
        {
            // internal ctor
        }

        /// <summary>
        ///     Connection string to connect to the storage account this have to able to perform write operations
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Table name to storage the message if not (messageslogsMMddyyyy) will be created as default
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     Partition name to be used with storage tables (messages) will be created as default
        /// </summary>
        public string PartitionKey { get; set; }
    }
}