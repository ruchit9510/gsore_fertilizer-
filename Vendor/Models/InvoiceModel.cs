using System;
using System.Collections.Generic; // Add this for List<T>
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vendor.Models
{
    public class InvoiceModel
    {
        [Key]
        public int ID { get; set; }

        public DateTime? DateInvoice { get; set; }
        public float Total_Bill { get; set; }

        public int? FKUserID { get; set; }
        [ForeignKey("FKUserID")]
        public virtual SignupLogin user { get; set; }

        // Add this navigation property for Orders
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}