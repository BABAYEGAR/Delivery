using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Data.Service.Enums
{
    public enum OrderStatus
    {
        New,
        [Display(Name = "In Progress")]
        InProgress,
        Delivered,
        Cancelled
    }
}
