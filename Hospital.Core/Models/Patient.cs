using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }


        // navigation propirty
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Billing> Billings { get; set; } = new List<Billing>();
        public ICollection<Laboratory> Laboratories { get; set; } = new List<Laboratory>();
        public ICollection<Inpatient_Admission> Inpatient_Admissions { get; set; } = new List<Inpatient_Admission>();
    }
}
