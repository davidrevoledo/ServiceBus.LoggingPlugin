# ServiceBus.LogginPlugin
Plugin to log Service bus messages

### This is an add-in for [Microsoft.Azure.ServiceBus client](https://github.com/Azure/azure-service-bus-dotnet/) 

Allow to log messages before being sent by tracing or saving in an Azure Storage Account.

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

      queueClient.RegisteredPlugins.Add(new LogginPlugin(configurations =>
      {
          configurations.LogginType = LogginType.Trace;
      }));
```        

### Using Storage Account Mode

Configuration and registration

```c#
      queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

      queueClient.RegisteredPlugins.Add(new LogginPlugin(configurations =>
      {
          configurations.LogginType = LogginType.StorageTable;
          configurations.SendInBackground = true;
          configurations.ConfigureStorageAccount(information =>
              {
                  information.ConnectionString = "Insert Storage Connection string";
              });
      }));
```  

### Simple Log Method

Configuration and registration

```c#
      queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

      queueClient.RegisteredPlugins.Add(new LogginPlugin(configurations =>
      {
          configurations.LogginType = LogginType.None;
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
      public class CustomLogginService : ILogginService
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

       queueClient.RegisteredPlugins.Add(new LogginPlugin(configurations =>
       {
            configurations.LogginType = LogginType.Custom;
            configurations.ServiceProvider = _serviceProvider;
            configurations.CustomLogginService = typeof(CustomLogginService);
       }));
```      
