using System;
using Hydra.Payment.Domain.Enumerables;

namespace Hydra.Payment.Facade.Models
{
    public class PaymentResult
    {
        public Guid TransactionID { get; set; }

        private EPaymentStatus _status;
        public EPaymentStatus Status 
        { 
            get => _status;
            set {
                if(_status == EPaymentStatus.failed){
                    //Send Payment failed to the queue
                }

                if(_status == EPaymentStatus.processed){
                    //Send Payment processed to the queue
                }
                _status = value;
            }
         }
        public RefuseReason RefusedReason { get; set; }
    }
}