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
        [Key]
        public  long OrderId { get; set; }
        [Required]
        public string Location { get; set; } 
        
        public long? ShishaId { get; set; } 
        [ForeignKey("ShishaId")]
        public virtual Shisha Shisha { get; set; }
        public long? FlavourId { get; set; }
        [ForeignKey("FlavourId")]
        public virtual Flavour Flavour  { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime DateOfOrder { get; set; }
        public DateTime DateOrderModified { get; set; }
        public string Mobile { get; set; }
        public string OrderStatus { get; set; }
        public string OrderCode { get; set; }
        public long? TotalAmount { get; set; }
        public long? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }

    }
}
