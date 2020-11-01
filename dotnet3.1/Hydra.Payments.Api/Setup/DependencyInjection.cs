using Hydra.Payments.Api.Facade;
using Hydra.Payments.Api.Infrastructure;
using Hydra.Payments.Api.Infrastructure.Data;
using Hydra.Payments.Api.Infrastructure.interfaces;
using Hydra.Payments.Api.Services;
using Hydra.WebAPI.Core.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Payments.Api.Setup
{
     public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentFacade, CreditCardPaymentFacade>();

            //Data
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<PaymentContext>();
        }
    }
}