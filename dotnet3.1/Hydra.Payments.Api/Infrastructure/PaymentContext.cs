using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hydra.Core.Data;
using Hydra.Core.Messages;
using Hydra.Payments.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Payments.Api.Infrastructure
{
    public class PaymentContext: DbContext, IUnitOfWork
    {

        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public DbSet<Payment> Payments {get;set;}
        public DbSet<Transaction> Transactions {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)"); // avoid do create any column NVarchar(MAX)

            modelBuilder.Ignore<ValidationResult>();
            //Use to Ignore event to persist on Database.
            modelBuilder.Ignore<Event>();

            //Does not need to add map for each element, new EF supports
            //It will find all entities and mapping defined on DbSet<TEntity> via reflection
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            } 

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit() => await base.SaveChangesAsync() > 0;
    }
}