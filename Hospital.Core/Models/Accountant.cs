using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Models
{
    public class Accountant
    {
        public int AccountantId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }


        // Navigation property
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<Billing> Billings { get; set; } = new List<Billing>();
       

    }
}
