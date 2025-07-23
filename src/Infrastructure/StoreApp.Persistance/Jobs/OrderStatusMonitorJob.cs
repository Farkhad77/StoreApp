using Microsoft.EntityFrameworkCore;
using StoreApp.Infrastructure.Services;
using StoreApp.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Jobs
{
    public class OrderStatusMonitorJob
    {
        private readonly StoreAppDbContext _context;
        private readonly IEmailService _emailService;

        public OrderStatusMonitorJob(StoreAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task CheckOrderStatusChangesAsync()
        {
            var orders = await _context.Orders
                .Where(o => o.OrderStatus != o.LastNotifiedStatus)
                .ToListAsync();

            foreach (var order in orders)
            {
                var user = await _context.Users.FindAsync(order.UserId);
                if (user == null) continue;

                var sent = await _emailService.SendEmailAsync(
                    user.Email,
                    "Sifariş statusu yeniləndi",
                    $"Yeni status: {order.OrderStatus}");

                if (sent)
                {
                    order.LastNotifiedStatus = order.OrderStatus;
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}
