using Microsoft.EntityFrameworkCore;
using SagePayServerIntegration.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagePayServerIntegration.DbContexts
{
    public class ApplicationContext : DbContext
    {
        public virtual DbSet<SagePayPaymentDetail> SagePayPaymentDetail { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SagePayPaymentDetail>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.BillingAddress1).HasMaxLength(4000);

                entity.Property(e => e.BillingAddress2).HasMaxLength(4000);

                entity.Property(e => e.BillingCity).HasMaxLength(4000);

                entity.Property(e => e.BillingCountry).HasMaxLength(4000);

                entity.Property(e => e.BillingFirstnames).HasMaxLength(4000);

                entity.Property(e => e.BillingPostCode).HasMaxLength(4000);

                entity.Property(e => e.BillingState).HasMaxLength(4000);

                entity.Property(e => e.BillingSurname).HasMaxLength(4000);

                entity.Property(e => e.CustomerEMail)
                    .HasColumnName("CustomerEMail")
                    .HasMaxLength(4000);

                entity.Property(e => e.DeliveryAddress1).HasMaxLength(4000);

                entity.Property(e => e.DeliveryAddress2).HasMaxLength(4000);

                entity.Property(e => e.DeliveryCity).HasMaxLength(4000);

                entity.Property(e => e.DeliveryCountry).HasMaxLength(4000);

                entity.Property(e => e.DeliveryFirstnames).HasMaxLength(4000);

                entity.Property(e => e.DeliveryPostCode).HasMaxLength(4000);

                entity.Property(e => e.DeliveryState).HasMaxLength(4000);

                entity.Property(e => e.DeliverySurname).HasMaxLength(4000);

                entity.Property(e => e.Status)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionCompleted).HasColumnType("datetime");

                entity.Property(e => e.VendorTxCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VPSSignature).HasColumnName("VPSSignature");

                entity.Property(e => e.VPSSignatureServerValue).HasColumnName("VPSSignatureServerValue");

                entity.Property(e => e.VPSTxId).HasColumnName("VPSTxId");

            });
            modelBuilder.Entity<Country>().HasData(
            new Country[]
            {
                new Country { Code="AD", Name="ANDORRA"},
                new Country { Code="AE", Name="UNITED ARAB EMIRATES"},
                new Country { Code="AF", Name="AFGHANISTAN"},
                new Country { Code="AG", Name="ANTIGUA AND BARBUDA"},
                new Country { Code="US", Name="UNITED STATES"}
            });
            modelBuilder.Entity<State>().HasData(
            new State[]
            {
                new State { Code="AK", Name="Alaska"},
                new State { Code="AL", Name="Alabama"},
                new State { Code="AR", Name="Arkansas"},
                new State { Code="AZ", Name="Arizona"},
            });
        }
    }
}
