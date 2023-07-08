using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CommonLibraries.Application.Logging
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (context, configuration) =>
           {
               Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
               var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
               var clientId = context.Configuration.GetValue<string>("ElasticConfiguration:ClientId");
               var apiKey = context.Configuration.GetValue<string>("ElasticConfiguration:ApiKey");
               var username = context.Configuration.GetValue<string>("ElasticConfiguration:Username");
               var password = context.Configuration.GetValue<string>("ElasticConfiguration:Password");
               var logIndex = context.Configuration.GetValue<string>("ElasticConfiguration:Index");
               int? numberOfShards = context.Configuration.GetValue<int>("ElasticConfiguration:Shards");
               int? replicas = context.Configuration.GetValue<int>("ElasticConfiguration:Replicas");
               var logFiles = bool.Parse(context.Configuration.GetValue<string>("ElasticConfiguration:LogFiles"));
               var logConsole = bool.Parse(context.Configuration.GetValue<string>("ElasticConfiguration:LogConsole"));
               var logElastic = bool.Parse(context.Configuration.GetValue<string>("ElasticConfiguration:LogElastic"));
               var dir = Directory.GetCurrentDirectory();
               var config = configuration
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName();
               if (logFiles)
                   config = config.WriteTo.File($"{dir}/Logs/log_{DateTime.Now:yyyy_mm_dd_HH_mm}.txt", rollingInterval: RollingInterval.Day);
               if (logConsole)
                   config = config.WriteTo.Console();
               if(logElastic)
                  config = config.WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticUri))
                        {
                            IndexFormat = $"{logIndex}-{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                            AutoRegisterTemplate = true,
                            ModifyConnectionSettings = (c) => {
                                if(username != null && password != null)
                                   return c.BasicAuthentication(username, password);
                                if(clientId != null && apiKey != null)
                                    return c.ApiKeyAuthentication(clientId,apiKey);
                                return c;
                             },
                            NumberOfShards = numberOfShards,
                            NumberOfReplicas = replicas
                        })
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                    .ReadFrom.Configuration(context.Configuration);
           };
    }
}
