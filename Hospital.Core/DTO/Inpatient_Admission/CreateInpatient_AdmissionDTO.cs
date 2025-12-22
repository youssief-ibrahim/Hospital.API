using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Inpatient_Admission
{
    public class CreateInpatient_AdmissionDTO
    {
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public DateTime? DischargeDate { get; set; }
        [JsonIgnore]
        public int PatientId { get; set; }
        public int DepartmentId { get; set; }
        public int RoomId { get; set; }
        [JsonIgnore]
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }
    }
}
