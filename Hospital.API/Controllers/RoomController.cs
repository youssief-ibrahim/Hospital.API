using AutoMapper;
using Hospital.Core.DTO.Room;
using Hospital.Core.DTO.Staff;
using Hospital.Core.IReposatory;
using Hospital.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IGenericReposatory<Room> Genroom;
        private readonly IGenericReposatory<Department> GenDept;
        private readonly IStringLocalizer<RoomController> localizer;

        private readonly IMapper mapper;
        public RoomController(IGenericReposatory<Room> Genroom, IMapper mapper, IGenericReposatory<Department> GenDept, IStringLocalizer<RoomController> localizer)
        {
            this.Genroom = Genroom;
            this.mapper = mapper;
            this.localizer = localizer;
            this.GenDept = GenDept;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Getall()
        {
            var res = await Genroom.GetAll();
            var data = mapper.Map<List<AllRoomDTO>>(res);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Getbyid(int id)
        {
            var res = await Genroom.GetById(s => s.RoomId == id);
            if (res == null) return NotFound("Room not found");
            var data = mapper.Map<AllRoomDTO>(res);
            return Ok(data);
        }
        [HttpGet("RoomNumber")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Getroomname(int roomnum)
         {
            var res = await Genroom.GetById(s => s.RoomNumber == roomnum);
            if (res == null) return NotFound("Room not found");
            var data = mapper.Map<AllRoomDTO>(res);
            return Ok(data);
         }
        [HttpGet("AllRoomAvilable")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Roomavilabee()
        {
            var res = await Genroom.FindAll(s => s.IsAvailable==true);
            var data=mapper.Map<List<AllRoomDTO>>(res);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Createroom(CreateRoomDTO createroom)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deptid = await GenDept.GetById(d => d.DepartmentId == createroom.DepartmentId);
            if (deptid == null) return NotFound("Department not found");

            var room = mapper.Map<Room>(createroom);
            await Genroom.Create(room);
            Genroom.Save();

            var res = mapper.Map<AllRoomDTO>(room);
            return Ok(res);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Updateroom(int id,CreateRoomDTO updateroom)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = await Genroom.GetById(s => s.RoomId == id);
            if (room == null) return NotFound("Room not found");

            var deptid = await GenDept.GetById(d => d.DepartmentId == updateroom.DepartmentId);
            if (deptid == null) return NotFound("Department not found");

            mapper.Map(updateroom, room);
            Genroom.Save();

            var res = mapper.Map<AllRoomDTO>(room);
            return Ok(res);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deleteroom(int id)
        {
            var room = await Genroom.GetById(s => s.RoomId == id);
            if (room == null) return NotFound("Room not found");

            Genroom.delete(room);
            Genroom.Save();

            var res = mapper.Map<AllRoomDTO>(room);
            return Ok(res);
        }
    }
}
