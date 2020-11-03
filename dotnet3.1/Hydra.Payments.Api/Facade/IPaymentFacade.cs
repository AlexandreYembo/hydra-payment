using System.Threading.Tasks;
using Hydra.Payments.Api.Models;

namespace Hydra.Payments.Api.Facade
{
    public interface IPaymentFacade
    {
        Task<Transaction> AuthorizePayment(Payment payment);
         Task<Transaction> CapturePayment(Transaction transaction);
         Task<Transaction> CancelPayment(Transaction transaction);
    }
}