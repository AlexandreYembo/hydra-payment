using Hydra.Payments.Api.Infrastructure;
using Hydra.Payments.Api.Infrastructure.Data;
using Hydra.Payments.Api.Infrastructure.interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Payments.Api.Setup
{
     public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //Data
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<PaymentContext>();
        }
    }
}