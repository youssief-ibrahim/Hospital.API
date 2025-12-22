using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Billing
{
    public class CreateBillingDTO
    {
        public int Amount { get; set; }
        public DateTime BillingDate { get; set; } = DateTime.Now;
        public int PatientId { get; set; }
        [JsonIgnore]
        public int AccountantId { get; set; }
    }
}
