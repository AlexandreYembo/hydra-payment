using System;
using System.Linq;
using Hydra.Payment.CrossCutting.Interfaces;
using Hydra.Payment.CrossCutting.Models;

namespace Hydra.Payment.CrossCutting.Gateways
{
    public class PayPalGateway : IPayPalGateway
    {
        public void BuildGateway(GatewayConfiguration configuration)
        {
            //Implement the Factory Pattern here.
           //throw new System.NotImplementedException();
        }

        public bool CommitTransaction(string cardHashKey, string orderId, decimal amount)
        {
            // Fake implementation
            return new Random().Next(2) == 0;
        }

        public string GetCardHashKey(string serviceKey, string creditCardNumber)
        {
            // Fake implementation
           return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10).Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public string GetPayPalServiceKey(GatewayConfiguration configuration)
        {
            // Fake implementation
           return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10).Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}