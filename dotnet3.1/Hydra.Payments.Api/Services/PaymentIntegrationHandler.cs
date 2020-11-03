using System;
using System.Threading;
using System.Threading.Tasks;
using Hydra.Core.DomainObjects;
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
            SetSubscribers();

            return Task.CompletedTask;
        }

        private void SetResponder() =>
            _messageBus.RespondAsync<OrderInProcessingIntegrationEvent, ResponseMessage>(async request =>
                                await AuthorizePayment(request));
        
        private void SetSubscribers()
        {
            //Listen from the queue OrderCanceled
            _messageBus.SubscribeAsync<OrderCanceledIntegrationEvent>("OrderCanceled", async request =>
                await CancelPayment(request));

            //Listen from the queue ItemRemovedFromStock
            _messageBus.SubscribeAsync<OrderItemRemovedFromStockIntegrationEvent>("ItemRemovedFromStock", async request =>
                await CapturePayment(request));
        }

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

        private async Task CapturePayment(OrderItemRemovedFromStockIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
            var response = await paymentService.CapturePayment(message.OrderId);

            if(!response.ValidResult.IsValid)
                throw new DomainException($"Error to process the payment for the order {message.OrderId}");
            
            //will be captured by the Order API
            await _messageBus.PublishAsync(new OrderPaidIntegrationEvent(message.CustomerId, message.OrderId));
        }

        /// <summary>
        /// OrderCanceledIntegrationEvent is listened for Payment API and Order API, it does not need to publish, only need to cancel the payment
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task CancelPayment(OrderCanceledIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
            var response = await paymentService.CancelPayment(message.OrderId);

            if(!response.ValidResult.IsValid)
                throw new DomainException($"Error to cancel the payment for the order {message.OrderId}");
        }
    }
}