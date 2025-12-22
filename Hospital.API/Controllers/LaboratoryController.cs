using System.Security.Claims;
using AutoMapper;
using Hospital.Core.DTO.Appointment;
using Hospital.Core.DTO.Laboratory;
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
    public class LaboratoryController : ControllerBase
    {
        private readonly IGenericReposatory<Laboratory> GenLaab;
        private readonly IGenericReposatory<Patient> GenPatint;
        private readonly IGenericReposatory<Doctor> GenDoctor;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<LaboratoryController> localizer;
        public LaboratoryController(IGenericReposatory<Laboratory> GenLaab, IGenericReposatory<Patient> GenPatint, IGenericReposatory<Doctor> GenDoctor, IMapper mapper, IStringLocalizer<LaboratoryController> localizer)
        {
            this.GenLaab = GenLaab;
            this.GenPatint = GenPatint;
            this.GenDoctor = GenDoctor;
            this.localizer = localizer;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Getall()
        {
            var res = await GenLaab.GetAll();
            var data = mapper.Map<List<AllLaboratoryDTO>>(res);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetByid(int id)
        {
            var res = await GenLaab.GetById(s => s.LaboratoryId == id);
            if (res == null) return NotFound("Laboratory not found");
            var data = mapper.Map<AllLaboratoryDTO>(res);
            return Ok(data);
        }
        [HttpGet("Doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetByDoctor()
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var doctor = await GenDoctor.GetById(d => d.UserId == userid);
            if (doctor == null) return NotFound("Doctor not found");

            var res = await GenLaab.FindAll(l => l.DoctorId == doctor.DoctorId);
            var data = mapper.Map<List<AllLaboratoryDTO>>(res);
            return Ok(data);
        }
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateLaboratory(CreateLaboratoryDTO createLaboratory)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
          if(userid == 0) return BadRequest("Invalid User");

          var doctor = await GenDoctor.GetById(d => d.UserId == userid);
          if (doctor == null) return NotFound("Doctor not found");

          var patient=await GenPatint.GetById(p => p.PatientId == createLaboratory.PatientId);
           if (patient == null) return NotFound("Patient not found");

            var laboratory = mapper.Map<Laboratory>(createLaboratory);
            laboratory.DoctorId = doctor.DoctorId;

            await GenLaab.Create(laboratory);
            GenLaab.Save();

            var data = mapper.Map<AllLaboratoryDTO>(laboratory);
            return Ok(data);
        }

        [HttpPut("{LaboratoryId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateLaporatory(int LaboratoryId, CreateLaboratoryDTO updatelaboratory)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var doctor = await GenDoctor.GetById(d => d.UserId == userid);
            if (doctor == null) return NotFound("Doctor not found");

            var patient = await GenPatint.GetById(p => p.PatientId == updatelaboratory.PatientId);
            if (patient == null) return NotFound("Patient not found");

            var laboratory = await GenLaab.GetById(l=>l.LaboratoryId== LaboratoryId && l.DoctorId==doctor.DoctorId);
            if (laboratory == null) return NotFound("Laboratory not found");

            mapper.Map(updatelaboratory, laboratory);
            laboratory.DoctorId = doctor.DoctorId;
            GenLaab.update(laboratory);
            GenLaab.Save();

            var data = mapper.Map<AllLaboratoryDTO>(laboratory);
            return Ok(data);
        }
        [HttpDelete("{LaboratoryId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateLaporatory(int LaboratoryId)
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var doctor = await GenDoctor.GetById(d => d.UserId == userid);
            if (doctor == null) return NotFound("Doctor not found");

            var laboratory = await GenLaab.GetById(l => l.LaboratoryId == LaboratoryId && l.DoctorId == doctor.DoctorId);
            if (laboratory == null) return NotFound("Laboratory not found");

            GenLaab.delete(laboratory);
            GenLaab.Save();

            var data = mapper.Map<AllLaboratoryDTO>(laboratory);
            return Ok(data);
        }
    }
}
