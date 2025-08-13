using Microsoft.EntityFrameworkCore;
using Dukaan.Models;  // अगर तुम्हारे Product, Order, OrderItem models इसी namespace में हैं

namespace Dukaan.Data    // ये namespace अपनी project structure के हिसाब से रखो
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
