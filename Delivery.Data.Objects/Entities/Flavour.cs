using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Delivery.Data.Objects.Entities
{
    public class Flavour
    {
        public  long FlavourId { get; set; }
        [Required]
        [DisplayName("Flavour Name")]
        public string Name { get; set; }
        [DisplayName("Available Quantity")]
        [Required]
        public int AvailableQuantity { get; set; }
        [DisplayName("Safety Stock")]
        [Required]
        public int SafetyStock { get; set; }
        [Required]
        [DisplayName("Unit Price")]
        public long UnitAmount { get; set; }
        public IEnumerable<Order> Order { get; set; }
        public IEnumerable<StockLog> StockLogs { get; set; }
    }
}
