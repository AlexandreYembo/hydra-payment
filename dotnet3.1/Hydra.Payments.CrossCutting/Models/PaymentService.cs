namespace Hydra.Payments.CrossCutting.Models
{
    public class PaymentService
    {
        public readonly string ApiKey;
        public readonly string EncryptionKey;

        public PaymentService(string apiKey, string encryptionKey)
        {
            ApiKey = apiKey;
            EncryptionKey = encryptionKey;
        }
    }
}