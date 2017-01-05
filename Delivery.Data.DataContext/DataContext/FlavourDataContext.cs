﻿using System.Data.Entity;

namespace Delivery.Data.DataContext.DataContext
{
    public class FlavourDataContext : DbContext
    {
        public FlavourDataContext() : base("name=DeliveryDb")
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public Objects.Entities.Flavour Flavour { get; set; }
        public Objects.Entities.Order Order { get; set; }

        public System.Data.Entity.DbSet<Delivery.Data.Objects.Entities.Flavour> Flavours { get; set; }
    }
}
