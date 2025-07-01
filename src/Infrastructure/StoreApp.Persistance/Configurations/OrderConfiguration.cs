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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.OrderDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(o => o.user)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderProducts)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
