using System;
using System.Threading.Tasks;
using Hydra.Payments.Api.Models;
using Hydra.Payments.CrossCutting.Enumerables;
using Hydra.Payments.CrossCutting.Models;
using Microsoft.Extensions.Options;
using Transaction = Hydra.Payments.Api.Models.Transaction;

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

        public static Transaction MapTransaction(CrossCutting.Models.Transaction transaction)
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
    }
}