using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Data.Objects.Entities
{
    public class Order
    {
        public  long OrderId { get; set; }
        public string Location { get; set; } 
        
        public long ShishaId { get; set; } 
        [ForeignKey("ShishaId")]
        public virtual Shisha Shisha { get; set; }
        public long FlavourId { get; set; }
        [ForeignKey("FlavourId")]
        public virtual Flavour Flavour  { get; set; }
        [Required]
        public int Quantity { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime DateOfOrder { get; set; }
        public string Mobile { get; set; }
        public string OrderStatus { get; set; }


    }
}
