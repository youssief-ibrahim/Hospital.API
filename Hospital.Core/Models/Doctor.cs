using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Gender { get; set; }
        public bool IsAvailable { get; set; }
        

        // navigation propirty
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Laboratory> Laboratories { get; set; } = new List<Laboratory>();
        public ICollection<Inpatient_Admission> Inpatient_Admissions { get; set; } = new List<Inpatient_Admission>();
    }
}
