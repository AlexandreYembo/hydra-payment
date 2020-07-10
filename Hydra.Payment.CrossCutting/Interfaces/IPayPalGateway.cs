using Hydra.Payment.CrossCutting.Models;

namespace Hydra.Payment.CrossCutting.Interfaces
{
    public interface IPayPalGateway : IGateway
    {
        string GetPayPalServiceKey(GatewayConfiguration configuration);
        string GetCardHashKey(string serviceKey, string creditCardNumber);
        bool CommitTransaction(string cardHashKey, string orderId, decimal amount);
    }
}