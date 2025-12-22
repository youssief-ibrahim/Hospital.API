using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Billing
    {
        public int BillingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillingDate { get; set; } = DateTime.Now;

        // Navigation property
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        public int AccountantId { get; set; }
        [ForeignKey("AccountantId")]
        public Accountant Accountant { get; set; }

    }
}
