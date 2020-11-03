using System;
using Hydra.Core.DomainObjects;
using Hydra.Payments.Api.Enumerables;

namespace Hydra.Payments.Api.Models
{
    public class Transaction : Entity
    {
        public string AuthorizationCode { get; set; }
        public string CardBrand { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
        public TransactionStatus Status { get; set; }

        /// <summary>
        /// Transaction Id --> Used for the gateway
        /// </summary>
        /// <value></value>
        public string Tid { get; set; }

        /// <summary>
        /// Identitification of the type of payment
        /// </summary>
        /// <value></value>
        public string Nsu { get; set; }
        public Guid PaymentId { get; set; }
        
        //EF Relationship
        public Payment Payment { get; set; }

    }
}