using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Core.Models
{
    public class ApplicationUser:IdentityUser<int>
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? RefrenceNewEmail { get; set; }
        public DateTime? CodeExpiry { get; set; }

        // navigation propirty
        public Accountant? Accountant { get; set; }
        public Doctor? Doctor { get; set; }
        public Staff? staff { get; set; } 
        public Patient? Patient { get; set; }
        public  List<RefreshToken>? RefreshTokens { get; set; }


    }
}
