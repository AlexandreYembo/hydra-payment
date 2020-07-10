using Hydra.Payment.Domain.Enumerables;

namespace Hydra.Payment.Domain.Models
{
    public class PaymentDetail
    {
        public EPaymentType Type { get; set; }
        public string Details { get; set; }

        //TODO implement the generic object for each type of payment used.
    }
}