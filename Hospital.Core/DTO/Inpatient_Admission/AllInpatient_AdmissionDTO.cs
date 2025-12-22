using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Inpatient_Admission
{
    public class AllInpatient_AdmissionDTO
    {
        public int Inpatient_AdmissionId { get; set; }
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public DateTime? DischargeDate { get; set; }
        public int PatientId { get; set; }
        public int DepartmentId { get; set; }
        public int RoomId { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }

    }
}
