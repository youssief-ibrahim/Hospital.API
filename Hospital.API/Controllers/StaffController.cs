using System.Security.Claims;
using AutoMapper;
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
    public class StaffController : ControllerBase
    {
        private readonly IGenericReposatory<Staff> Genstaff;
        private readonly IGenericReposatory<Department> GenDept;
        private readonly IStringLocalizer<StaffController> localizer;

        private readonly IMapper mapper;
        public StaffController(IGenericReposatory<Staff> Genstaff, IMapper mapper, IGenericReposatory<Department> GenDept, IStringLocalizer<StaffController> localizer)
        {
            this.Genstaff = Genstaff;
            this.mapper = mapper;
            this.localizer = localizer;
            this.GenDept = GenDept;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Getall()
        {
            var res = await Genstaff.GetAll();
            var data = mapper.Map<List<AllstaffDTO>>(res);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Getbyid(int id)
        {
            var res = await Genstaff.GetById(s => s.StaffId == id);
            if (res == null) return NotFound("Staff not found");
            var data = mapper.Map<AllstaffDTO>(res);
            return Ok(data);
        }
        [HttpGet("Search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Searchstaff([FromQuery] string Name)
        {
            string name=Name.ToLower().Trim();
            var res = await Genstaff.GetAllwithsearch(s => s.Name.Contains(name)|| s.Position.Contains(name),s=>s.Department);
            var data = mapper.Map<List<AllstaffDTO>>(res);
            return Ok(data);
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Createstaff(CreatestaffDTO createstaff)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var deptid = await GenDept.GetById(d => d.DepartmentId == createstaff.DepartmentId);
            if (deptid == null) return NotFound("Department not found");

            var staff = mapper.Map<Staff>(createstaff);
            staff.UserId = userId;

            await Genstaff.Create(staff);
            Genstaff.Save();

            var res = mapper.Map<AllstaffDTO>(staff);
            return Ok(res);
        }
        [HttpPut]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Updatestaff(CreatestaffDTO updatestaff)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var staff = await Genstaff.GetById(s => s.UserId == userId);
            if (staff == null) return NotFound("Staff not found");

            var deptid = await GenDept.GetById(d => d.DepartmentId == updatestaff.DepartmentId);
            if (deptid == null) return NotFound("Department not found");

            mapper.Map(updatestaff, staff);
            staff.UserId = userId;

            Genstaff.update(staff);
            Genstaff.Save();

            var res = mapper.Map<AllstaffDTO>(staff);
            return Ok(res);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deletestaff(int id)
        {
            var staff = await Genstaff.GetById(s => s.StaffId == id);
            if (staff == null) return NotFound("Staff not found");

            Genstaff.delete(staff);
            Genstaff.Save();
            return Ok("Staff deleted successfully");
        }
    }
}
