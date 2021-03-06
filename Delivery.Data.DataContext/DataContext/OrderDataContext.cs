﻿using System.Data.Entity;
using Delivery.Data.Objects.Entities;

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

        public DbSet<Order> Orders { get; set; }

        public DbSet<Flavour> Flavours { get; set; }

        public DbSet<Shisha> Shishas { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<StockLog> StockLogs { get; set; }
    }
}
