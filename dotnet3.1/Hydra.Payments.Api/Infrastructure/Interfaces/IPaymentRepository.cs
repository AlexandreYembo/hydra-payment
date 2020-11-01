using Hydra.Core.Data;
using Hydra.Payments.Api.Models;

namespace Hydra.Payments.Api.Infrastructure.interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
         void AddPayment(Payment payment);
    }
}