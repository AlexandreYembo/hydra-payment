using Hydra.Payment.Domain.Models;

namespace Hydra.Payment.Domain.Interfaces
{
    public interface IPayment
    {
         Models.Payment ProcessPayment(Order order, Models.Payment payment);
    }
}