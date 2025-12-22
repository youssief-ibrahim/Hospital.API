using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }

        // navigation propirty
        public ICollection<Doctor> Doctors { get; set; }= new List<Doctor>();
        public ICollection<Staff> Staff { get; set; } = new List<Staff>();
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Inpatient_Admission> Inpatient_Admissions { get; set; } = new List<Inpatient_Admission>();

    }
}
