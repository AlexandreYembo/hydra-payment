using System.Threading.Tasks;
using FluentValidation.Results;
using Hydra.Core.Integration.Messages;
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

            if(transacion.Status != CrossCutting.Enumerables.TransactionStatus.Authorized)
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
    }
}