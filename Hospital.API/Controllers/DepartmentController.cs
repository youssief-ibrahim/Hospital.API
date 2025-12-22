using System.Security.Claims;
using AutoMapper;
using Hospital.Core.DTO.Department;
using Hospital.Core.DTO.Patient;
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
    public class DepartmentController : ControllerBase
    {
        private readonly IGenericReposatory<Department> GenDept;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<DepartmentController> localizer;
        public DepartmentController(IGenericReposatory<Department> GenDept, IMapper mapper, IStringLocalizer<DepartmentController> localizer)
        {
            this.GenDept = GenDept;
            this.mapper = mapper;
            this.localizer = localizer;
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var res = await GenDept.GetAll();
            var data = mapper.Map<List<AllDepartmentDTO>>(res);
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Getbyid(int id)
        {
            var res = await GenDept.GetById(s => s.DepartmentId == id);
            if (res == null) return NotFound("Department not found");
            var data = mapper.Map<AllDepartmentDTO>(res);
            return Ok(data);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDTO createDepartment)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var department = mapper.Map<Department>(createDepartment);
            await GenDept.Create(department);
            GenDept.Save();

            var res = mapper.Map<AllDepartmentDTO>(department);
            return Ok(res);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment(int id,CreateDepartmentDTO createDepartment)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var department = await GenDept.GetById(p => p.DepartmentId ==id);
            if(department==null) return NotFound("Department not found");

            mapper.Map(createDepartment, department);
            GenDept.update(department);
            GenDept.Save();

            var res = mapper.Map<AllDepartmentDTO>(department);
            return Ok(res);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await GenDept.GetById(p => p.DepartmentId==id);
            if (department == null) return NotFound("Department not found");

            GenDept.delete(department);
            GenDept.Save();

            var res = mapper.Map<AllDepartmentDTO>(department);
            return Ok(res);
        }
    }
}
