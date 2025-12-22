using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Laboratory
    {
        public int LaboratoryId { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }= DateTime.Now;
        public decimal Cost { get; set; }

        // navigation propirty
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }
    }
}
