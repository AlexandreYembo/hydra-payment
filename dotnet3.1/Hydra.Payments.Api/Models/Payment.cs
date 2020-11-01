using System;
using System.Collections.Generic;
using Hydra.Core.DomainObjects;
using Hydra.Payments.CrossCutting.Enumerables;

namespace Hydra.Payments.Api.Models
{
    public class Payment : Entity, IAggregateRoot
    {
        public Payment()
        {
            Transactions = new List<Transaction>();
        }

        public Guid OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Price { get; set; }

        public CreditCard CreditCard { get; set; }

        //EF Relationship
        public ICollection<Transaction> Transactions { get; set; }

        public void AddTransaction(Transaction transaction) => Transactions.Add(transaction);
    }
}