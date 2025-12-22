using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Billing
{
    public class AllBillingDTO
    {
        public int BillingId { get; set; }
        public int Amount { get; set; }
        public DateTime BillingDate { get; set; } = DateTime.Now;
        public int PatientId { get; set; }
        public int AccountantId { get; set; }
    }
}
