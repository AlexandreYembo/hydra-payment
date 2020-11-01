using Hydra.Core.Data;
using Hydra.Payments.Api.Infrastructure.interfaces;
using Hydra.Payments.Api.Models;

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

        public void AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void Dispose() => _context.Dispose();
    }
}