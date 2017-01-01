﻿using System.Data.Entity;

namespace Delivery.Data.DataContext.DataContext
{
    public class DeliveryDataContext : DbContext
    {
        public DeliveryDataContext() : base("name=DeliveryDb")
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public Objects.Entities.Delivery Deliver { get; set; }
        public Objects.Entities.Shisha Shisha { get; set; }
        public Objects.Entities.Flavour Flavour { get; set; }
    }
}
