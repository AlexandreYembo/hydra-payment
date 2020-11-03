using System;
using System.Threading.Tasks;
using Hydra.Payments.Api.Models;
using Hydra.Payments.CrossCutting.Enumerables;
using Hydra.Payments.CrossCutting.Models;
using Microsoft.Extensions.Options;
using Transaction = Hydra.Payments.Api.Models.Transaction;
using TransactionStatus = Hydra.Payments.Api.Enumerables.TransactionStatus;

namespace Hydra.Payments.Api.Facade
{
    public class CreditCardPaymentFacade : IPaymentFacade
    {
        private readonly PaymentConfig _config;

        public CreditCardPaymentFacade(IOptions<PaymentConfig> paymentConfig)
        {
            _config = paymentConfig.Value;
        }
        public async Task<Transaction> AuthorizePayment(Payment payment)
        {
            var paymentService = new PaymentService(_config.DefaultApiKey, _config.DefaultEncryptionKey);

            var cardHashGen = new CardHash(paymentService)
            {
                CardNumber = payment.CreditCard.CardNumber,
                CardHolderName = payment.CreditCard.CardName,
                CardExpirationDate = payment.CreditCard.Expiration,
                CardCvv = payment.CreditCard.CVV
            };

            var cardHash = cardHashGen.Generate();

            var transaction = new CrossCutting.Models.Transaction(paymentService)
            {
                CardHash = cardHash,
                CardNumber = payment.CreditCard.CardNumber,
                CardHolderName = payment.CreditCard.CardName,
                CardExpirationDate = payment.CreditCard.Expiration,
                CardCvv = payment.CreditCard.CVV,
                PaymentMethod = CrossCutting.Enumerables.PaymentMethod.CreditCard,
                Amount = payment.Price
            };
 
            return MapTransaction(await transaction.AuthorizeCardTransaction());
        }

        public async Task<Transaction> CapturePayment(Transaction transaction)
        {
            var service = new PaymentService(_config.DefaultApiKey, _config.DefaultEncryptionKey);

            var transactionGateway = MapToGateway(transaction, service);

            return MapTransaction(await transactionGateway.CaptureCardTransaction());
        }

        public async Task<Transaction> CancelPayment(Transaction transaction)
        {
             var service = new PaymentService(_config.DefaultApiKey, _config.DefaultEncryptionKey);

            var transactionGateway = MapToGateway(transaction, service);

            return MapTransaction(await transactionGateway.CancelAuthorization());
        }

        public Transaction MapTransaction(CrossCutting.Models.Transaction transaction)
        {
            return new Transaction
            {
                Id = Guid.NewGuid(),
                Status = (TransactionStatus)transaction.Status,
                Amount = transaction.Amount,
                CardBrand = transaction.CardBrand,
                AuthorizationCode = transaction.AuthorizationCode,
                Cost = transaction.Cost,
                TransactionDate = transaction.TransactionDate,
                Nsu = transaction.Nsu,
                Tid = transaction.Tid
            };
        }

        public CrossCutting.Models.Transaction MapToGateway(Transaction transaction, PaymentService service)
        {
            return new CrossCutting.Models.Transaction(service)
            {
                Status = (CrossCutting.Enumerables.TransactionStatus) transaction.Status,
                Amount = transaction.Amount,
                CardBrand = transaction.CardBrand,
                AuthorizationCode = transaction.AuthorizationCode,
                Cost = transaction.Cost,
                Nsu = transaction.Nsu,
                Tid = transaction.Tid
            };
        }
    }
}