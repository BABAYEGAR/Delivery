using System.Collections.Generic;
using System.ComponentModel;

namespace Delivery.Data.Objects.Entities
{
    public class Flavour
    {
        public  long FlavourId { get; set; }
        public string Name { get; set; }
        [DisplayName("Available Quantity")]
        public int AvailableQuantity { get; set; }
        [DisplayName("Safety Stock")]
        public int SafetyStock { get; set; }
        public IEnumerable<Order> Order { get; set; }
    }
}
