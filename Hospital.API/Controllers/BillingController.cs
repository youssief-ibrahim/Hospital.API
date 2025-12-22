using System.Security.Claims;
using AutoMapper;
using Hospital.Core.DTO.Accountant;
using Hospital.Core.DTO.Billing;
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
    public class BillingController : ControllerBase
    {
        private readonly IGenericReposatory<Billing> GensBilling;
        private readonly IGenericReposatory<Accountant> GensAccountant;
        private readonly IGenericReposatory<Patient> GensPatient;
        private readonly IStringLocalizer<BillingController> localizer;

        private readonly IMapper mapper;
        public BillingController(IGenericReposatory<Accountant> GensAccountant, IGenericReposatory<Billing> GensBilling, IGenericReposatory<Patient> GensPatient,IMapper mapper, IStringLocalizer<BillingController> localizer)
        {
            this.GensAccountant = GensAccountant;
            this.GensBilling = GensBilling;
            this.GensPatient = GensPatient;
            this.mapper = mapper;
            this.localizer = localizer;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> Getall()
        {
            var res = await GensBilling.GetAll();
            if (res.Count == 0) return NotFound("No Billing found");
            var data = mapper.Map<List<AllBillingDTO>>(res);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> Getbyid(int id)
        {
            var res = await GensBilling.GetById(s => s.BillingId == id);
            if (res == null) return NotFound("Billing not found");
            var data = mapper.Map<AllBillingDTO>(res);
            return Ok(data);
        }
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Admin,Accountant,Patient")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var res = await GensBilling.FindAll(b => b.PatientId == patientId);
            if (res.Count == 0) return NotFound("No Billing found for this patient");
            var data = mapper.Map<List<AllBillingDTO>>(res);
            return Ok(data);
        }
        
        [HttpPost]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> CreateBilling(CreateBillingDTO createBilling)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var accountant = await GensAccountant.GetById(a => a.UserId == userId);
            if (accountant == null) return NotFound("Accountant not found");

            var patient = await GensPatient.GetById(p => p.PatientId == createBilling.PatientId);
            if (patient == null) return NotFound("Patient not found");

            var billing = mapper.Map<Billing>(createBilling);
            billing.AccountantId = accountant.AccountantId;

            await GensBilling.Create(billing);
            GensBilling.Save();

            var res = mapper.Map<AllBillingDTO>(billing);
            return Ok(res);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> UpdateBilling(int id, CreateBillingDTO updateBilling)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var accountant = await GensAccountant.GetById(a => a.UserId == userId);
            if (accountant == null) return NotFound("Accountant not found");

            var billing = await GensBilling.GetById(b => b.BillingId == id);
            if (billing == null) return NotFound("Billing not found");

            accountant.AccountantId = billing.AccountantId;
            mapper.Map(updateBilling, billing);

            GensBilling.update(billing);
            GensBilling.Save();

            var res = mapper.Map<AllBillingDTO>(billing);
            return Ok(res);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> DeleteBilling(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0) return Unauthorized();

            var accountant = await GensAccountant.GetById(a => a.UserId == userId);
            if (accountant == null) return NotFound("Accountant not found");

            var billing = await GensBilling.GetById(b => b.BillingId == id);
            if (billing == null) return NotFound("Billing not found");

            GensBilling.delete(billing);
            GensBilling.Save();
            return Ok("Billing deleted successfully");
        }
    }
}
