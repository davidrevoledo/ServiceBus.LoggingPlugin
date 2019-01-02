<p align="center">
  <img src="servicebuslogging.jpg" alt="Service bus Logging Plugin" width="100"/>
</p>


# ServiceBus.LoggingPlugin
Plugin to log Service bus messages

### This is an add-in for [Microsoft.Azure.ServiceBus client](https://github.com/Azure/azure-service-bus-dotnet/) 

Allow to log messages before being sent by tracing or saving in an Azure Storage Account Table.

[![Build status](https://ci.appveyor.com/api/projects/status/p8v6u2dud236vshu?svg=true)](https://ci.appveyor.com/project/davidrevoledo/servicebus-logginplugin)
[![CodeFactor](https://www.codefactor.io/repository/github/davidrevoledo/servicebus.logginplugin/badge)](https://www.codefactor.io/repository/github/davidrevoledo/servicebus.logginplugin)
![NuGet](https://img.shields.io/nuget/dt/ServiceBus.LogginPlugin.svg)
![NuGet](https://img.shields.io/nuget/v/ServiceBus.LogginPlugin.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

### Nuget package

To Install from the Nuget Package Manager Console 

```sh
PM > Install-Package ServiceBus.LogginPlugin 
NET CLI - dotnet add package ServiceBus.LogginPlugin
paket paket add ServiceBus.LogginPlugin
```
Available here https://www.nuget.org/packages/ServiceBus.LogginPlugin

    
## Examples
### Use Tracing method 

Configuration and registration

```c#
      queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

      queueClient.RegisteredPlugins.Add(new LoggingPlugin(configurations =>
      {
          configurations.LoggingType = LoggingType.Trace;
      }));
```        

### Using Storage Account Mode

Configuration and registration

```c#
      queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

      queueClient.RegisteredPlugins.Add(new LoggingPlugin(configurations =>
      {
          configurations.LoggingType = LoggingType.StorageTable;
          configurations.SendInBackground = true;
          configurations.ConfigureStorageAccount(information =>
              {
                  information.ConnectionString = "Insert Storage Connection string";
              });
      }));
```  

Optional Configurations
``` Custom Partition key ```
``` Custom storage table name ```

``` c#
 configurations.ConfigureStorageAccount(information =>
                {
                    information.ConnectionString = "Insert Storage Connection string";
                    information.PartitionKey = "YourPartitionKey";
                    information.TableName = "YourTableName";
                });
```

### Simple Log Method

Configuration and registration

```c#
      queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

      queueClient.RegisteredPlugins.Add(new LoggingPlugin(configurations =>
      {
          configurations.LoggingType = LoggingType.None;
          configurations.Log = message =>
          {
              // you can handle manually the message here
              var m1 = message;
          };
      }));
```  

### Use Custom LoggerService 

Configuration and registration

```c#
      public class CustomLoggingService : ILoggingService
      {
          public void Dispose()
          {
          }

          public bool Disposed { get; set; }

          public Task LogSentMessage(Message message)
          {
              // implement your logic here
          }

          public void SetConfigurations(ILogginConfigurations configurations)
          {
          }
      }
      
       queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
       queueClient.RegisterLoggingService(new CustomLoggingService());

```      

### Icon
Created by [Mariana Alemanno](https://www.behance.net/mariana-alemanno)

Made with ❤ in [DGENIX](https://www.dgenix.com/)
