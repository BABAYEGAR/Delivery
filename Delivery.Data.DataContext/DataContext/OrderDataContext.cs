using System.Data.Entity;

namespace Delivery.Data.DataContext.DataContext
{
    public class    OrderDataContext : DbContext
    {
        public OrderDataContext() : base("name=DeliveryDb")
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public Objects.Entities.Order Order { get; set; }
        public Objects.Entities.Shisha Shisha { get; set; }
        public Objects.Entities.Flavour Flavour { get; set; }

        public System.Data.Entity.DbSet<Delivery.Data.Objects.Entities.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<Delivery.Data.Objects.Entities.Flavour> Flavours { get; set; }

        public System.Data.Entity.DbSet<Delivery.Data.Objects.Entities.Shisha> Shishas { get; set; }
    }
}
