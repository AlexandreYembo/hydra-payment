using Hydra.Payment.Domain.Interfaces;
using Hydra.Payment.Domain.Models;
using Hydra.Payment.Facade.Interfaces;

namespace Hydra.Payment.Services
{
    /// <summary>
    /// class responsble to process the payment via interface IPayment that is implemented by Payment Facade
    /// </summary>
    public class PaymentService : IPayment
    {
       private readonly IPaymentFacade _paymentFacade;

       public PaymentService(IPaymentFacade paymentFacade){
           _paymentFacade = paymentFacade;
       }

        public Domain.Models.Payment Process(Order order, Domain.Models.Payment payment)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Submit the request to process the payment
        /// </summary>
        /// <param name="order"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        public Domain.Models.Payment ProcessPayment(Order order, Domain.Models.Payment payment)
       {
           //TODO: Implement any logic or validation
           var result =  _paymentFacade.ProcessPayment(order, payment);
           payment.Status = result.Status;
           payment.TransactionCodeConfirmation = result.TransactionID.ToString();
           
           return payment;
       }
    }
}