using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Exporter;


namespace Common.Libraries.Services.tracing
{
    public static class TracingDependencies
    {
        public static void RegisterTracingServices(this IServiceCollection services, IConfiguration Configuration, string serviceName, string[] sources)
        {
            services.AddOpenTelemetryTracing((builder) => builder
           .SetResourceBuilder(ResourceBuilder.CreateDefault()
               .AddService(serviceName))
           .AddSource(sources)
           .AddAspNetCoreInstrumentation()
           /*.AddZipkinExporter(zipkinOptions =>
           {
               zipkinOptions.Endpoint = new Uri(Configuration.GetValue<string>("Zipkin:Endpoint"));
           })*/
           .AddConsoleExporter()
           
           );
        }
    }
}
