using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Staff
{
    public class CreatestaffDTO
    {
        public string Name { get; set; }
        public string Position { get; set; } // Nurse, Technician, Administrative Staff
        public DateOnly HireDate { get; set; }= DateOnly.FromDateTime(DateTime.Now);

        public int Salary { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
    }
}
