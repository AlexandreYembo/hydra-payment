using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Core.Data;
using Hydra.Payments.Api.Models;

namespace Hydra.Payments.Api.Infrastructure.interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
         void AddPayment(Payment payment);
        Task<List<Transaction>> GetTransactionByOrderId(Guid orderId);
        void AddTransaction(Transaction transaction);
    }
}