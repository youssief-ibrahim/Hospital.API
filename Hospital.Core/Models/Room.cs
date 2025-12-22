using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Range(1, 1000)]
        public int RoomNumber { get; set; }
        [Range(1, 10)]
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }

        // navigation propirty
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        public ICollection<Inpatient_Admission> Inpatient_Admissions { get; set; } = new List<Inpatient_Admission>();
    }
}
