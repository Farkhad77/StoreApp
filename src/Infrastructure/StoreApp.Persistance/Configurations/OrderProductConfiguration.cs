using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Configurations
{
    public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(op => op.Id);

            builder.Property(op => op.OrderCount)
                .IsRequired();

            builder.Property(op => op.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

          
        }
    }
}
