using System.Security.Claims;
using AutoMapper;
using Hospital.Core.DTO.Appointment;
using Hospital.Core.DTO.Staff;
using Hospital.Core.IReposatory;
using Hospital.Core.Models;
using Hospital.EF.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IGenericReposatory<Appointment> GenAppoint;
        private readonly IGenericReposatory<Patient> GenPatint;
        private readonly IGenericReposatory<Doctor> GenDoctor;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<AppointmentController> localizer;
        public AppointmentController(IGenericReposatory<Appointment> GenAppoint, IGenericReposatory<Patient> GenPatint, IGenericReposatory<Doctor> GenDoctor, IMapper mapper, IStringLocalizer<AppointmentController> localizer)
        {
            this.GenAppoint = GenAppoint;
            this.GenPatint = GenPatint;
            this.GenDoctor = GenDoctor;
            this.mapper = mapper;
            this.localizer = localizer;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Getall()
        {
            var res = await GenAppoint.GetAll();
            var data = mapper.Map<List<AllAppointmentDTO>>(res);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByid(int id)
        {
            var res = await GenAppoint.GetById(s=>s.AppointmentId==id);
            if (res == null) return NotFound("Appointment not found");
            var data = mapper.Map<AllAppointmentDTO>(res);
            return Ok(data);
        }
        [HttpGet("PatientId")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> getpatintid()
        {
            var userid= int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var patient = await GenPatint.GetById(p => p.UserId == userid);
            if(patient == null) return NotFound("Patient not found");

            var res = await GenAppoint.FindAll(s => s.PatientId == patient.UserId);
            if (res == null) return NotFound("Patient not found with this id");

            var data = mapper.Map<List<AllAppointmentDTO>>(res);
            return Ok(data);
        }
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentDTO createAppointment)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var patient = await GenPatint.GetById(p => p.UserId == userid);
            if (patient == null) return NotFound("Patient not found");

            var doctor = await GenDoctor.checkId(createAppointment.DoctorId);
            if (doctor == null) return NotFound("Doctor not found");

            if(doctor.IsAvailable == false) return BadRequest("Doctor is not available");
            var appointment = mapper.Map<Appointment>(createAppointment);
            appointment.PatientId = patient.PatientId;
            if(appointment.AppointmentDate < DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("Appointment date cannot be in the past");
            }

            await GenAppoint.Create(appointment);
            GenAppoint.Save();

            var res = mapper.Map<AllAppointmentDTO>(appointment);
            return Ok(res);
        }
        [HttpPut("{appointmentId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateAppointment(int appointmentId, CreateAppointmentDTO updateAppointment)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var patient = await GenPatint.GetById(p => p.UserId == userid);
            if (patient == null) return NotFound("Patient not found");

            var doctor = await GenDoctor.checkId(updateAppointment.DoctorId);
            if (doctor == null) return NotFound("Doctor not found");
            if (doctor.IsAvailable == false) return BadRequest("Doctor is not available");

            var appointment = await GenAppoint.GetById(
             a => a.AppointmentId == appointmentId &&
            a.PatientId == patient.PatientId
             );
            if (appointment == null) return NotFound("Appointment not found");
            mapper.Map(updateAppointment, appointment);
            appointment.PatientId = patient.PatientId;
            if (appointment.AppointmentDate < DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("Appointment date cannot be in the past");
            }

            GenAppoint.update(appointment);
            GenAppoint.Save();

            var res = mapper.Map<AllAppointmentDTO>(appointment);
            return Ok(res);
        }
        [HttpDelete("{appointmentId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DeleteAppointment( int appointmentId)
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var patient = await GenPatint.GetById(p => p.UserId == userid);
            if (patient == null) return NotFound("Patient not found");

            var appointment = await GenAppoint.GetById(
             a => a.AppointmentId == appointmentId &&
            a.PatientId == patient.PatientId
             );
            if (appointment == null) return NotFound("Appointment not found");

            GenAppoint.delete(appointment);
            GenAppoint.Save();

           var res = mapper.Map<AllAppointmentDTO>(appointment);
            return Ok(res);
        }
    }
}
