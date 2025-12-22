using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public DateOnly AppointmentDate { get; set; }

        [RegularExpression("Pending|Completed|Admitted",
          ErrorMessage = "Status must be: Pending, Completed, or Admitted")]
        public string Status { get; set; } = "Pending";

        // Navigation property
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public ICollection<Inpatient_Admission>? Inpatient_Admissions { get; set; }

    }
}
