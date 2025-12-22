using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Core.Models;

namespace Hospital.Core.DTO.Staff
{
    public class AllstaffDTO
    {
        public int StaffId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; } // Nurse, Technician, Administrative Staff
        public DateOnly HireDate { get; set; }= DateOnly.FromDateTime(DateTime.Now);

        public int Salary { get; set; }

        public int UserId { get; set; }
        public int DepartmentId { get; set; }
    }
}
