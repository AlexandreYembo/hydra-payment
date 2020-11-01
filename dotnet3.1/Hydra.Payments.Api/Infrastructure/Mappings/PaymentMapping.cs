using Hydra.Payments.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hydra.Payments.Api.Infrastructure.Mappings
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            //It will ignore and won't persist on Database
            builder.Ignore(p => p.CreditCard);


            //1 : N => Payment : Transaction
            builder.HasMany(p => p.Transactions)
                .WithOne(t => t.Payment)
                .HasForeignKey(t => t.PaymentId);

            builder.ToTable("Payments");
        }
    }
}