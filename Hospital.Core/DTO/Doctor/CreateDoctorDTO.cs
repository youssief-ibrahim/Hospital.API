using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Doctor
{
    public class CreateDoctorDTO
    {
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Gender { get; set; }
        public bool IsAvailable { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
    }
}
