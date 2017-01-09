using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Data.Service.Enums
{
    public  enum  UserType
    {
        Administrator,
        [Display(Name = "Delivery Man")]
        DeliveryMan,
        [Display(Name = "Procurement Officer")]
        ProcurementOfficer,
        Accountant,
        User
    }
}
