using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hydra.Payment.CrossCutting.Models
{
  public class CardHash
    {
        public CardHash(PaymentService paymentService)
        {
            PaymentService = paymentService;
        }

        private readonly PaymentService PaymentService;

        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardCvv { get; set; }

        /// <summary>
        /// Simulate to generate the hash of the gateway. This is not used for real scenario.
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            using var aesAlg = Aes.Create();

            aesAlg.IV = Encoding.Default.GetBytes(PaymentService.EncryptionKey);
            aesAlg.Key = Encoding.Default.GetBytes(PaymentService.ApiKey);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(CardHolderName + CardNumber + CardExpirationDate + CardCvv);
            }

            return Encoding.ASCII.GetString(msEncrypt.ToArray());
        }
    }
}