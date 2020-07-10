namespace Hydra.Payment.Domain.Enumerables
{
    public enum EPaymentStatus
    {
        failed = -99,
        started = 0,
        inProgress = 1,

        validatingFund = 2,
        processed = 3
    }
}