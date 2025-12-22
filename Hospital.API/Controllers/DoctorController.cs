using System.Security.Claims;
using AutoMapper;
using Hospital.Core.DTO.Department;
using Hospital.Core.DTO.Doctor;
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
    public class DoctorController : ControllerBase
    {
        private readonly IGenericReposatory<Doctor> GenDoctor;
        private readonly IGenericReposatory<Department> GenDept;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<DoctorController> localizer;
        public DoctorController(IGenericReposatory<Doctor> GenDoctor, IMapper mapper, IGenericReposatory<Department> GenDept, IStringLocalizer<DoctorController> localizer)
        {
            this.GenDoctor = GenDoctor;
            this.mapper = mapper;
            this.GenDept = GenDept;
            this.localizer = localizer;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Getall()
        {
            var res = await GenDoctor.GetAll();
            var data = mapper.Map<List<AllDoctorDTO>>(res);
            return Ok(data);
        }
        [HttpGet("Departmet/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetallwithDepartment(int id)
        {
            var res = await GenDoctor.FindAll(s => s.DepartmentId == id,s=>s.Department);
            var data = mapper.Map<List<DoctorDepartmentDTO>>(res);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Getbyid(int id)
        {
            var res = await GenDoctor.GetById(s => s.DoctorId == id);
            if (res == null) return NotFound("Doctor not found");
            var data = mapper.Map<AllDoctorDTO>(res);
            return Ok(data);
        }
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Createdoctor(CreateDoctorDTO createDoctor)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var deptid = await GenDept.GetById(d => d.DepartmentId == createDoctor.DepartmentId);
            if (deptid == null) return NotFound("Department not found");

            var doctor = mapper.Map<Doctor>(createDoctor);
            doctor.UserId = userId;

            await GenDoctor.Create(doctor);
            GenDoctor.Save();

            var res = mapper.Map<AllDoctorDTO>(doctor);
            return Ok(res);
        }
        [HttpPut]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateDoctor(CreateDoctorDTO createDoctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var doctor = await GenDoctor.GetById(p => p.UserId == userId);
            if (doctor == null) return NotFound("Doctor not found");

            var deptid = await GenDept.GetById(d => d.DepartmentId == createDoctor.DepartmentId);
            if (deptid == null) return NotFound("Department not found");

            mapper.Map(createDoctor, doctor);
            doctor.UserId = userId;

            GenDoctor.update(doctor);
            GenDoctor.Save();

            var res = mapper.Map<AllDoctorDTO>(doctor);
            return Ok(res);
        }
        [HttpDelete]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DeleteDoctor()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var doctor = await GenDoctor.GetById(p => p.UserId == userId);
            if (doctor == null) return NotFound("Doctor not found");

            GenDoctor.delete(doctor);
            GenDoctor.Save();

            return Ok("Doctor deleted successfully");
        }
    }
}
