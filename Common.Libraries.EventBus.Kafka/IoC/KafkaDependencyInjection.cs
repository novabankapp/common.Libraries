
using Common.Libraries.EventBus.Kafka.Consumer;
using Common.Libraries.EventBus.Kafka.Interfaces;
using Common.Libraries.EventBus.Kafka.Producer;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Libraries.EventBus.Kafka.IoC
{
    public class KafkaDependencyInjection
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration Configuration)
        {
			

			var producerConfig = new ProducerConfig(new ClientConfig
            {
                BootstrapServers = Configuration["Kafka:ClientConfigs:BootstrapServers"]
            });

            services.AddSingleton(producerConfig);
            services.AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));

			var clientConfig = new ClientConfig()
			{
				BootstrapServers = Configuration["Kafka:ClientConfigs:BootstrapServers"]
			};

			
			var consumerConfig = new ConsumerConfig(clientConfig)
			{
				GroupId = Configuration["Kafka:ClientConfigs:GroupId"],
				EnableAutoCommit = bool.Parse(Configuration["Kafka:ClientConfigs:EnableAutoCommit"]),
				AutoOffsetReset = AutoOffsetReset.Earliest,
				StatisticsIntervalMs = int.Parse(Configuration["Kafka:ClientConfigs:StatisticsIntervalMs"]),
				SessionTimeoutMs = int.Parse(Configuration["Kafka:ClientConfigs:SessionTimeoutMs"])
			};

			services.AddSingleton(producerConfig);
			services.AddSingleton(consumerConfig);

			services.AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));

			
			services.AddSingleton(typeof(IKafkaConsumer<,>), typeof(KafkaConsumer<,>));
			
			
		}
    }
}
