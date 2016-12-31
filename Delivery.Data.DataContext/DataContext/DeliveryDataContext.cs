using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Data.DataContext.DataContext
{
    public class DeliveryDataContext : DbContext
    {
        public DeliveryDataContext() : base("Delivery")
        {
            
        }
        
    }
}
