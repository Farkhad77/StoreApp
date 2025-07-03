using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities;

public class User :  IdentityUser
{
   

    public string FullName { get; set; }
    public string? RefreshToken { get; set; } 
    public DateTime? ExpiryDate { get; set; } // Refresh token üçün istifadə olunur

    // Navigation
    public ICollection<Product> Products { get; set; }
    public ICollection<Favorite> Favorites { get; set; }


}
