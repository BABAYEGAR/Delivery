using System.Data.Entity;
using Delivery.Data.Objects.Entities;

namespace Delivery.Data.DataContext.DataContext
{
    public class StockLogDataContext : DbContext
    {
        public StockLogDataContext() : base("name=DeliveryDb")
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public DbSet<StockLog> StockLogs { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Flavour> Flavours { get; set; }

        public DbSet<Shisha> Shishas { get; set; }
    }
}
