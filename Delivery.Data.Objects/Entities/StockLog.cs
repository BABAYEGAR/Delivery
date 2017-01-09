using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Data.Objects.Entities
{
    public class StockLog
    {
        public  long StockLogId { get; set; }
        [Display(Name = "Item Category")]
        public string ItemCategory { get; set; }
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        public string Action { get; set; }
        [Display(Name = "Amount")]
        public long Amount { get; set; }
        public long? FlavourId { get; set; }
        [ForeignKey("FlavourId")]
        public  virtual Flavour Flavour { get; set; }
        public long? ShishaId { get; set; }
        [ForeignKey("ShishaId")]
        public virtual Shisha Shisha { get; set; }

        public DateTime ActionDate { get; set; }
    }
}
