using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Data.Objects.Entities
{
    public class Shisha
    {
        public long ShishaId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Order> Order { get; set; }
 
    }
}
