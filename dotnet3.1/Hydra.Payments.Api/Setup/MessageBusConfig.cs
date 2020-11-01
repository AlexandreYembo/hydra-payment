using Hydra.Core.Extensions;
using Hydra.Core.MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Payments.Api.Setup
{
   public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
            //         .AddHostedService<BasketIntegrationHandler>();
        }
    }
}