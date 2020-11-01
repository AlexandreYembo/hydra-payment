using System.Threading.Tasks;
using Hydra.Payments.Api.Models;

namespace Hydra.Payments.Api.Facade
{
    public interface IPaymentFacade
    {
        Task<Transaction> AuthorizePayment(Payment payment);
    }
}