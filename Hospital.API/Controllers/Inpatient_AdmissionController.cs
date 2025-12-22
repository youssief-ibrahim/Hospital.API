using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Hospital.Core.DTO.Inpatient_Admission;
using Hospital.Core.DTO.Laboratory;
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
    public class Inpatient_AdmissionController : ControllerBase
    {
        private readonly IGenericReposatory<Inpatient_Admission> GenInpatient;
        private readonly IGenericReposatory<Department> GenDep;
        private readonly IGenericReposatory<Patient> GenPatint;
        private readonly IGenericReposatory<Doctor> GenDoctor;
        private readonly IGenericReposatory<Room> GenRoom;
        private readonly IGenericReposatory<Appointment> GenAppoint;
        private readonly IStringLocalizer<Inpatient_AdmissionController> localizer;
        private readonly ApplicationDbContext contextl;

        private readonly IMapper mapper;
        public Inpatient_AdmissionController(IGenericReposatory<Inpatient_Admission> GenInpatient, IGenericReposatory<Department> GenDep, IGenericReposatory<Patient> GenPatint, IGenericReposatory<Doctor> GenDoctor, IGenericReposatory<Room> GenRoom, IGenericReposatory<Appointment> GenAppoint, IMapper mapper, ApplicationDbContext contextl, IStringLocalizer<Inpatient_AdmissionController> localizer)
        {
            this.GenInpatient = GenInpatient;
            this.GenDep = GenDep;
            this.GenPatint = GenPatint;
            this.GenDoctor = GenDoctor;
            this.GenRoom = GenRoom;
            this.GenAppoint = GenAppoint;
            this.mapper = mapper;
            this.localizer = localizer;
            this.contextl = contextl;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Getall()
        {
            var res = await GenInpatient.GetAll();
            var data = mapper.Map<List<AllInpatient_AdmissionDTO>>(res);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByid(int id)
        {
            var res = await GenInpatient.GetById(s => s.Inpatient_AdmissionId == id);
            if (res == null) return NotFound("Inpatient_Admission not found");
            var data = mapper.Map<AllInpatient_AdmissionDTO>(res);
            return Ok(data);
        }
        [HttpGet("DoctorAdmissions")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAdmissionsForDoctor()
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var doctor = await GenDoctor.GetById(d => d.UserId == userid);
            if (doctor == null) return NotFound("Doctor not found");

            var res = await GenInpatient.FindAll(i => i.DoctorId == doctor.DoctorId);
            if (res.Count == 0) return NotFound("No admissions found for this doctor");

            var data = mapper.Map<List<AllInpatient_AdmissionDTO>>(res);
            return Ok(data);
        }
        [HttpGet("PatientAdmissions/{PatientId}")]
        public async Task<IActionResult> GetAdmissionsForPatient(int PatientId)
        {
            var patient = await GenPatint.GetById(p => p.PatientId == PatientId);
            if (patient == null) return NotFound("Patient not found");
            var res = await GenInpatient.FindAll(i => i.PatientId == PatientId);
            if(res.Count == 0) return NotFound("No admissions found for this patient");
            var data = mapper.Map<List<AllInpatient_AdmissionDTO>>(res);
            return Ok(data);
        }


        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateInPatientAdmission(CreateInpatient_AdmissionDTO createInpatient)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var doctor = await GenDoctor.GetById(d => d.UserId == userid);
            if (doctor == null) return NotFound("Doctor not found");

            var appoiment = await GenAppoint.GetById(a => a.AppointmentId == createInpatient.AppointmentId);
            if (appoiment == null) return NotFound("Appointment not found");

            var exists = await GenInpatient.GetById(
              i => i.AppointmentId == createInpatient.AppointmentId &&
                i.DischargeDate == null
             );

            if (exists != null)
                return BadRequest("This appointment already has an active admission");
            if (appoiment.Status != "Pending") return BadRequest("Only pending appointments can be admitted");
            if (appoiment.DoctorId != doctor.DoctorId)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not assigned to this appointment");

            var room = await GenRoom.GetById(r => r.RoomId == createInpatient.RoomId);
            if (room == null) return NotFound("Room not found");
            if (!room.IsAvailable) return BadRequest("Room is not available");

            var dept = await GenDep.GetById(d => d.DepartmentId == createInpatient.DepartmentId);
            if (dept == null) return NotFound("Department not found");

            var inpatient = mapper.Map<Inpatient_Admission>(createInpatient);
            inpatient.DoctorId = doctor.DoctorId;
            inpatient.PatientId = appoiment.PatientId;
            inpatient.AdmissionDate = DateTime.Now;

            using var transaction = await contextl.Database.BeginTransactionAsync();

            await GenInpatient.Create(inpatient);
            GenInpatient.Save();

            room.IsAvailable = false;
            GenRoom.update(room);
            GenRoom.Save();

            appoiment.Status = "Admitted";
            GenAppoint.update(appoiment);
            GenAppoint.Save();

            await transaction.CommitAsync();

            var data = mapper.Map<AllInpatient_AdmissionDTO>(inpatient);
            return Ok(data);
        }
        [HttpPut("discharged/{Inpatient_AdmissionId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DischargeInPatientAdmission(int Inpatient_AdmissionId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var doctor = await GenDoctor.GetById(d => d.UserId == userid);
            if (doctor == null) return NotFound("Doctor not found");

            var inpatient = await GenInpatient.GetById(i => i.Inpatient_AdmissionId == Inpatient_AdmissionId);
            if (inpatient == null) return NotFound("Inpatient_Admission not found");
            if (inpatient.DischargeDate != null) return BadRequest("Patient already discharged");

            var room = await GenRoom.GetById(r => r.RoomId == inpatient.RoomId);
            if (room == null) return NotFound("Room not found");

            var appoiment = await GenAppoint.GetById(a => a.AppointmentId == inpatient.AppointmentId);
            if (appoiment == null) return NotFound("Appointment not found");
            inpatient.DischargeDate = DateTime.Now;

            using var transaction = await contextl.Database.BeginTransactionAsync();

            GenInpatient.update(inpatient);
            GenInpatient.Save();

            room.IsAvailable = true;
            GenRoom.update(room);
            GenRoom.Save();

            appoiment.Status = "Completed";
            GenAppoint.update(appoiment);
            GenAppoint.Save();

            await transaction.CommitAsync();

            var data = mapper.Map<AllInpatient_AdmissionDTO>(inpatient);
            return Ok(data);
        }
        [HttpPut("cancel/{Inpatient_AdmissionId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CancelInPatientAdmission(int Inpatient_AdmissionId)
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var doctor = await GenDoctor.GetById(d => d.UserId == userid);
            if (doctor == null) return NotFound("Doctor not found");

            var inpatient = await GenInpatient.GetById(i => i.Inpatient_AdmissionId == Inpatient_AdmissionId);
            if (inpatient == null) return NotFound("Inpatient_Admission not found");
            if (inpatient.DischargeDate != null) return BadRequest("Cannot cancel a discharged admission");
            if(inpatient.DoctorId != doctor.DoctorId)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not assigned to this admission");

            var room = await GenRoom.GetById(r => r.RoomId == inpatient.RoomId);
            if (room == null) return NotFound("Room not found");

            var appoiment = await GenAppoint.GetById(a => a.AppointmentId == inpatient.AppointmentId);
            if (appoiment == null) return NotFound("Appointment not found");

            using var transaction = await contextl.Database.BeginTransactionAsync();

            GenInpatient.delete(inpatient);
            GenInpatient.Save();

            room.IsAvailable = true;
            GenRoom.update(room);
            GenRoom.Save();

            appoiment.Status = "Pending";
            GenAppoint.update(appoiment);
            GenAppoint.Save();

            await transaction.CommitAsync();

            //var data = mapper.Map<AllInpatient_AdmissionDTO>(inpatient);
            //return Ok(data);

            return Ok("Inpatient admission cancelled successfully");
        }
    }
}
