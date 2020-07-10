using System;

namespace Hydra.Payment.Domain.Models
{
    public class Products
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

    }
}