using System.Data.Entity;

namespace Delivery.Data.DataContext.DataContext
{
    public class ShishaDataContext : DbContext
    {
        public ShishaDataContext() : base("name=DeliveryDb")
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public Objects.Entities.Shisha Shisha { get; set; }
        public Objects.Entities.Order Order { get; set; }

        public System.Data.Entity.DbSet<Delivery.Data.Objects.Entities.Shisha> Shishas { get; set; }
    }
}
