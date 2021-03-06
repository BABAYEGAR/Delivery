﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Delivery.Data.Objects.Entities
{
    public class Shisha
    {
        public long ShishaId { get; set; }
        [Required]
        [DisplayName("Shisha Name")]
        public string Name { get; set; }
        [DisplayName("Available Quantity")]
        [Required]
        public int AvailableQuantity { get; set; }
        [DisplayName("Safety Stock")]
        [Required]
        public int SafetyStock { get; set; }
        public string Image { get; set; }
        [Required]
        [DisplayName("Unit Price")]
        public long UnitAmount { get; set; }
        public IEnumerable<StockLog> StockLogs { get; set; }
 
    }
}
