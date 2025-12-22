using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.DTO.Room
{
    public class CreateRoomDTO
    {
        [Range(1, 1000, ErrorMessage = "Out of Range max Room number is 1000")]
        public int RoomNumber { get; set; }
        [Range(1, 10, ErrorMessage = "Out of Range max Capacity is 10")]
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public int DepartmentId { get; set; }
    }
}
