using System.Security.Claims;
using AutoMapper;
using Hospital.Core.DTO.Accountant;
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
    public class AccountantController : ControllerBase
    {
        private readonly IGenericReposatory<Accountant> GensAccountant;

        private readonly IMapper mapper;

        private readonly IStringLocalizer<AccountantController> localizer;

        public AccountantController(IGenericReposatory<Accountant> GensAccountant, IMapper mapper, IStringLocalizer<AccountantController> localizer)
        {
            this.GensAccountant = GensAccountant;
            this.mapper = mapper;
            this.localizer = localizer;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Getall()
        {
            var res = await GensAccountant.GetAll();
            if(res.Count==0) return NotFound("No Accountants found");
            var data = mapper.Map<List<AllAccountantDTO>>(res);
            return Ok(data);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Getbyid(int id)
        {
            var res = await GensAccountant.GetById(s => s.AccountantId == id);
            if (res == null) return NotFound("Accountant not found");
            var data = mapper.Map<AllAccountantDTO>(res);
            return Ok(data);
        }
        [HttpPost]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> CreateAccountant(CreateAccountantDTO createAccountant)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var accountant = mapper.Map<Accountant>(createAccountant);
            accountant.UserId = userId;

            await GensAccountant.Create(accountant);
            GensAccountant.Save();

            var res = mapper.Map<AllAccountantDTO>(accountant);
            return Ok(res);
        }

        [HttpPut]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> CUpdateAccountant(CreateAccountantDTO updateAccountant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var accountant = await GensAccountant.GetById(p => p.UserId == userId);
            mapper.Map(updateAccountant, accountant);

            accountant.UserId = userId;
            GensAccountant.update(accountant);
            GensAccountant.Save();

            var res = mapper.Map<AllAccountantDTO>(accountant);
            return Ok(res);
        }
        [HttpDelete("{id}")]
        [Authorize] // role added for security
        public async Task<IActionResult> DeleteAccountant(int id)
        {
            var accountant = await GensAccountant.GetById(a => a.AccountantId == id);
            if (accountant == null) return NotFound("Accountant not found");

            GensAccountant.delete(accountant);
            GensAccountant.Save();
            return Ok("Accountant deleted successfully");
        }
    }
}
