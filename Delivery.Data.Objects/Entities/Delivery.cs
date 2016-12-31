using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Data.Objects.Entities
{
    public class Delivery : Transport
    {
        public  long DeliveryId { get; set; }
        public string Location { get; set; } 
        public long ShishaId { get; set; } 
        [ForeignKey("ShishaId")]
        public virtual Shisha Shisha { get; set; }
        public long FlavourId { get; set; }
        [ForeignKey("FlavourId")]
        public virtual Flavour Flavour  { get; set; }

    }
}
