using Hydra.Payments.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hydra.Payments.Api.Infrastructure.Mappings
{
    public class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            //1 : N => Payment : Transaction
            builder.HasOne(t => t.Payment)
                .WithMany(p => p.Transactions);

            builder.ToTable("Transactions");
        }
    }
}