using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Laboratory
{
    public class CreateLaboratoryDTO
    {
        public string TestName { get; set; }
        public DateTime TestDate { get; set; } = DateTime.Now;
        public decimal Cost { get; set; }
        public int PatientId { get; set; }
        [JsonIgnore]
        public int DoctorId { get; set; }
    }
}
