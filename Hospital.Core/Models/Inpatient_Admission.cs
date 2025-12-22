using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Inpatient_Admission  //  االحجز داخل المستشفي
    {
        public int Inpatient_AdmissionId { get; set; }
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public DateTime? DischargeDate { get; set; }


       // navigation propirty

        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        public int AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }

    }
}
