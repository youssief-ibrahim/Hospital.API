using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Appointment
{
    public class AllAppointmentDTO
    {
        public int AppointmentId { get; set; }
        public DateOnly AppointmentDate { get; set; }= DateOnly.FromDateTime(DateTime.Now);

        [RegularExpression("Pending|Completed|Admitted",
               ErrorMessage = "Status must be: Pending, Completed, or Admitted")]
        public string Status { get; set; } = "Pending";

        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
