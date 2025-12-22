using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
    public class PatientController : ControllerBase
    {
        private readonly IGenericReposatory<Patient> GenPatint;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<PatientController> localizer;
        public PatientController(IGenericReposatory<Patient> GenPatint, IMapper mapper, IStringLocalizer<PatientController> localizer)
        {
            this.GenPatint = GenPatint;
            this.localizer = localizer;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Getall()
        {
            var res = await GenPatint.GetAll();
            var data = mapper.Map<List<AllPatientDTO>>(res);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Getbyid(int id)
        {
            var res = await GenPatint.GetById(s => s.PatientId == id);
            if (res == null) return NotFound("Patient not found");
            var data = mapper.Map<AllPatientDTO>(res);
            return Ok(data);
        }
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Createpatent(CreatePatientDTO createPatient)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var patient = mapper.Map<Patient>(createPatient);
            patient.UserId = userid;

            await GenPatint.Create(patient);
            GenPatint.Save();

            var res = mapper.Map<AllPatientDTO>(patient);
            return Ok(res);
        }
        [HttpPut]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdatePaient(CreatePatientDTO createPatient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var patient = await GenPatint.GetById(p => p.UserId == userid);

            mapper.Map(createPatient, patient);
            patient.UserId = userid;

            GenPatint.update(patient);
            GenPatint.Save();

            var res = mapper.Map<AllPatientDTO>(patient);
            return Ok(res);
        }
        [HttpDelete]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DeletePaient()
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var patient = await GenPatint.GetById(p => p.UserId == userid);
            if (patient == null) return NotFound("Patient not found");

            GenPatint.delete(patient);
            GenPatint.Save();

            var res = mapper.Map<AllPatientDTO>(patient);
            return Ok(res);
        }

    }
}
