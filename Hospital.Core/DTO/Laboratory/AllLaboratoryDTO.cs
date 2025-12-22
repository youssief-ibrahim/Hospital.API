using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Laboratory
{
    public class AllLaboratoryDTO
    {
        public int LaboratoryId { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; } = DateTime.Now;
        public decimal Cost { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
