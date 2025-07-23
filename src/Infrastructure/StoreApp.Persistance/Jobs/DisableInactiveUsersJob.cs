using Microsoft.AspNetCore.Identity;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Jobs
{
    public class DisableInactiveUsersJob
    {
        private readonly UserManager<User> _userManager;

        public DisableInactiveUsersJob(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task DisableUsersAsync()
        {
            var allUsers = _userManager.Users.ToList();

            foreach (var user in allUsers)
            {
                // Əgər user heç vaxt login olmayıbsa, və ya 6 ay keçibsə
                if (!user.LastLoginDate.HasValue || user.LastLoginDate.Value < DateTime.UtcNow.AddMonths(-6))
                {
                    // Hesabı blokla
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTimeOffset.MaxValue;

                    await _userManager.UpdateAsync(user);
                }
            }

            Console.WriteLine("✅ Inaktiv istifadəçilər donduruldu");
        }

    }

}
