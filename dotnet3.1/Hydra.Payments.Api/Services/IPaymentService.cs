using System;
using System.Threading.Tasks;
using Hydra.Core.Integration.Messages;
using Hydra.Payments.Api.Models;

namespace Hydra.Payments.Api.Services
{
    public interface IPaymentService
    {
         Task<ResponseMessage> AuthorizePayment(Payment payment);

         Task<ResponseMessage> CapturePayment(Guid orderId);
         Task<ResponseMessage> CancelPayment(Guid orderId);
    }
}