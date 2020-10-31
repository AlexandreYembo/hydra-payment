namespace Hydra.Payment.CrossCutting.Enumerables
{
     public enum TransactionStatus
    {
        Authorized = 1,
        Paid,
        Refused,
        Chargedback,
        Cancelled
    }
}