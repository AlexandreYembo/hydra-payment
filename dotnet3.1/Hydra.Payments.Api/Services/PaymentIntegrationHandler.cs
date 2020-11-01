using System;
using System.Threading;
using System.Threading.Tasks;
using Hydra.Core.Integration.Messages;
using Hydra.Core.Integration.Messages.OrderMessages;
using Hydra.Core.MessageBus;
using Hydra.Payments.Api.Models;
using Hydra.Payments.CrossCutting.Enumerables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hydra.Payments.Api.Services
{
    public class PaymentIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;

        public PaymentIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();

            return Task.CompletedTask;
        }

        private void SetResponder() =>
            _messageBus.RespondAsync<OrderInProcessingIntegrationEvent, ResponseMessage>(async request =>
                                await AuthorizePayment(request));

        private async Task<ResponseMessage> AuthorizePayment(OrderInProcessingIntegrationEvent message)
        {
            ResponseMessage response;

            using var scope = _serviceProvider.CreateScope();
            var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

            var payment = new Payment
            {
                OrderId = message.OrderId,
                PaymentMethod = (PaymentMethod) message.PaymentType,
                Price = message.Price,
                CreditCard = new CreditCard(message.CardName, message.CardNumber, message.Expiration, message.CVV)
            };

            response = await paymentService.AuthorizePayment(payment);

            return response;
        }
    }
}