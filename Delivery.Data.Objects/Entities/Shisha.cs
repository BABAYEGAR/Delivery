using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Delivery.Data.Objects.Entities
{
    public class Shisha
    {
        public long ShishaId { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Available Quantity")]
        public int AvailableQuantity { get; set; }
        [DisplayName("Safety Stock")]
        public int SafetyStock { get; set; }
        public IEnumerable<Order> Order { get; set; }
 
    }
}
