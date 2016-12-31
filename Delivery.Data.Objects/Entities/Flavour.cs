﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Data.Objects.Entities
{
    public class Flavour
    {
        public  long FlavourId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Delivery> Deliveries { get; set; }
    }
}
