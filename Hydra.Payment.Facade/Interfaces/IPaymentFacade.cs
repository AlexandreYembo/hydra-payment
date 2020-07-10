using Hydra.Payment.Domain.Models;
using Hydra.Payment.Facade.Models;

namespace Hydra.Payment.Facade.Interfaces
{
    public interface IPaymentFacade
    {
        PaymentResult ProcessPayment(Order order, Domain.Models.Payment payment);    
    }
}