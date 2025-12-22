using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; } // Nurse, Technician, Administrative Staff
        public DateOnly HireDate { get; set; }

        public int Salary { get; set; }

        // realationship
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
    }
}
