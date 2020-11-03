namespace Hydra.Payments.Api.Enumerables
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