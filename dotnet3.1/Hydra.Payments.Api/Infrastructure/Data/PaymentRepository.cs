using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Core.Data;
using Hydra.Payments.Api.Infrastructure.interfaces;
using Hydra.Payments.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Payments.Api.Infrastructure.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public PaymentRepository(PaymentContext context)
        {
            _context = context;
        }

        public void AddPayment(Payment payment) =>
            _context.Payments.Add(payment);

        public void AddTransaction(Transaction transaction) =>
            _context.Transactions.Add(transaction);

        public async Task<List<Transaction>> GetTransactionByOrderId(Guid orderId) => 
            await _context.Transactions.AsNoTracking()
                                   .Where(t => t.Payment.OrderId == orderId)
                                   .ToListAsync();

        public void Dispose() => _context.Dispose();
    }
}