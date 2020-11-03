using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hydra.Core.DomainObjects;
using Hydra.Core.Integration.Messages;
using Hydra.Payments.Api.Enumerables;
using Hydra.Payments.Api.Facade;
using Hydra.Payments.Api.Infrastructure.interfaces;
using Hydra.Payments.Api.Models;

namespace Hydra.Payments.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentFacade _paymentFacade;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentFacade paymentFacade, IPaymentRepository paymentRepository)
        {
            _paymentFacade = paymentFacade;
            _paymentRepository = paymentRepository;
        }

        public async Task<ResponseMessage> AuthorizePayment(Payment payment)
        {
            var transacion = await _paymentFacade.AuthorizePayment(payment);

            var validationResult = new ValidationResult();

            if(transacion.Status != TransactionStatus.Authorized)
            {
                validationResult.Errors.Add(new ValidationFailure("Payment", "Error to process your payment, please contact your card operator"));
                
                return new ResponseMessage(validationResult);
            }

            payment.AddTransaction(transacion);

            _paymentRepository.AddPayment(payment);

            if(!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Payment", "Error to process your payment"));

                //TODO: implement the process to revert the payment, request to Gateway. 
                //you can send to the Queue in case  you need to to more operation
                // OR if you only want to revert, you can simply call this method _paymentRepository.RevertPayment(transacion.Nsu);

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> CapturePayment(Guid orderId)
        {
            var transactions = await _paymentRepository.GetTransactionByOrderId(orderId);
            var authorizedTransaction = transactions?.FirstOrDefault(t => t.Status == TransactionStatus.Authorized);
            var validationResult = new ValidationResult();

            if(authorizedTransaction == null) throw new DomainException($"Transaction not found for the order {orderId}");

            var transaction = await _paymentFacade.CapturePayment(authorizedTransaction);

            if(transaction.Status != TransactionStatus.Paid)
            {
                validationResult.Errors.Add(new ValidationFailure("Payment", $"An error occurred to process the payment for the order {orderId}"));
                return new ResponseMessage(validationResult);
            }

            transaction.PaymentId = authorizedTransaction.PaymentId;
            _paymentRepository.AddTransaction(transaction);

            if(!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Payment", $"An error occurred to process the payment for the order {orderId}"));
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> CancelPayment(Guid orderId)
        {
            var transactions = await _paymentRepository.GetTransactionByOrderId(orderId);
            var authorizedTransaction = transactions?.FirstOrDefault(t => t.Status == TransactionStatus.Authorized);
            var validationResult = new ValidationResult();

            if(authorizedTransaction == null) throw new DomainException($"Transaction not found for the order {orderId}");

            var transaction = await _paymentFacade.CancelPayment(authorizedTransaction);

            if(transaction.Status != TransactionStatus.Cancelled)
            {
                validationResult.Errors.Add(new ValidationFailure("Payment", $"An error occurred to cancel the payment for the order {orderId}"));
                return new ResponseMessage(validationResult);
            }

            transaction.PaymentId = authorizedTransaction.PaymentId;
            _paymentRepository.AddTransaction(transaction);

            if(!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Payment", $"An error occurred to cancel the payment for the order {orderId}"));
            }

            return new ResponseMessage(validationResult);
        }
    }
}