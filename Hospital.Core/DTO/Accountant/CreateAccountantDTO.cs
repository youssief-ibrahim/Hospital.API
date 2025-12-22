using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Accountant
{
    public class CreateAccountantDTO
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
