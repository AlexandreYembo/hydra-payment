namespace Hydra.Payments.Api.Models
{
    public class CreditCard
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        
        protected CreditCard() {}

        public CreditCard(string cardName, string cardNumber, string expiration, string cVV)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            Expiration = expiration;
            CVV = cVV;
        }
    }
}