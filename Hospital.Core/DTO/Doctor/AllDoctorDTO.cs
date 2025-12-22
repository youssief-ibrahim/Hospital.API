using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Core.Models;

namespace Hospital.Core.DTO.Doctor
{
    public class AllDoctorDTO
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Gender { get; set; }
        public bool IsAvailable { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
    }
}
