using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Patient
{
    public class CreatePatientDTO
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
