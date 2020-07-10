using System;
using System.Collections.Generic;
using Hydra.Payment.CrossCutting.Interfaces;
using Hydra.Payment.CrossCutting.Models;
using Hydra.Payment.Domain.Enumerables;
using Hydra.Payment.Domain.Models;
using Hydra.Payment.Facade.Interfaces;
using Hydra.Payment.Facade.Models;

namespace Hydra.Payment.Facade
{
    public class PaymentCreditCardFacade : IPaymentFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        //Implement interface repository to get the configuration

        public PaymentCreditCardFacade(IPayPalGateway payPalGateway){
            _payPalGateway = payPalGateway;
        }
        
        public PaymentResult ProcessPayment(Order order, Domain.Models.Payment payment)
        {
            var config = new GatewayConfiguration();
            config.Id = Guid.NewGuid();
            config.GatewayName = "PayPal";
            config.Keys = new List<GatewayConfigurationKeys>
            {
                new GatewayConfigurationKeys{
                    Id = Guid.NewGuid(),
                    Key = "API_KEY",
                    Value = "FFF34432FFEEFDAUUU"
                },
                new GatewayConfigurationKeys{
                    Id = Guid.NewGuid(),
                    Key = "ENCRIPTION_KEY",
                    Value = "FFF34432FFEEFDAUU15616516516161==!343434@4343444ewefdsfdfd"
                },
            };

            _payPalGateway.BuildGateway(config);
            
            var serviceKey =  _payPalGateway.GetPayPalServiceKey(config);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, payment.PaymentDetail.Details);
            var paymentResult = _payPalGateway.CommitTransaction(cardHashKey, order.Id.ToString(), order.Amount);

            return new PaymentResult{
                TransactionID = Guid.NewGuid(),
                Status = paymentResult ? EPaymentStatus.processed : EPaymentStatus.failed,
                }; //implement from Payment facade transaction
        }
    }
}