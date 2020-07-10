using System;
using System.Collections.Generic;

namespace Hydra.Payment.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public List<Products> Products { get; set; }
    }
}