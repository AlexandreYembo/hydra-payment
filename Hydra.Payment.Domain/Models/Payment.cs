using Hydra.Payment.Domain.Enumerables;

namespace Hydra.Payment.Domain.Models
{
    public class Payment
    {
        public EPaymentStatus Status { get; set; }
        public decimal Value { get; set; }
        public string TransactionCodeConfirmation { get; set; }

        public PaymentDetail PaymentDetail {get; set; }
    }
}