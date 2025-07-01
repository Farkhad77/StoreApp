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
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasKey(f => f.Id);

            // Name sütunu əgər istifadə olunmursa, silinməlidir. Qalsa belə yazılır:
            builder.Property(f => f.Name);

            builder.HasOne(f => f.Product)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
