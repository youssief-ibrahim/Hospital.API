using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Hospital.Core.DTO.Auth;
using Hospital.Core.IReposatory;
using Hospital.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IEmailService emailService;
        private readonly IStringLocalizer<AccountController> localizer;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IEmailService emailService, IStringLocalizer<AccountController> localizer)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailService = emailService;
            this.localizer = localizer;
        }
        [HttpPost("Regiser")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await userManager.FindByEmailAsync(registerDTO.Email)!=null)
                return BadRequest(localizer["email"].Value);

            if (await userManager.FindByNameAsync(registerDTO.UserName)!=null)
                return BadRequest(localizer["username"].Value);

            if (!new EmailAddressAttribute().IsValid(registerDTO.Email))
                return BadRequest(localizer["invalidemailformate"].Value);

            ApplicationUser user = new ApplicationUser() { 
             Name=registerDTO.Name,
             UserName=registerDTO.UserName,
             Email=registerDTO.Email,
            }; 
            var result = await userManager.CreateAsync(user,registerDTO.Password);
            if(result.Succeeded)
            {
                ApplicationUser users = new ApplicationUser();
                Random generator = new Random();
                string code = generator.Next(0, 1000000).ToString("D6");
                user.Code = code;
                user.CodeExpiry=DateTime.Now.AddMinutes(5);
                await userManager.UpdateAsync(user);

                await emailService.SendEmailAsync(
                    Email: registerDTO.Email,
                    subject: localizer["confirmationEmail"].Value,
                    body: $"{localizer["confirmEmailCode"].Value} {code}"
                    );
                await userManager.AddToRoleAsync(user, "Accountant");
                return Ok($"{localizer["accountCreatedCheckEmail"].Value} \"{registerDTO.Email}\" .");
            }

            foreach (var item in result.Errors)
                ModelState.AddModelError("Password", item.Description);

            return BadRequest(ModelState);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> Confirmmail(string email,string code)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user==null)
                return NotFound(localizer["usernotfound"].Value);

            if (user.Code!=code)
                return BadRequest(localizer["invalidcode"].Value);

            if (user.CodeExpiry < DateTime.Now)
                return BadRequest(localizer["codeexpired"].Value);

            var token=await userManager.GenerateEmailConfirmationTokenAsync(user);
            var result=await userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
            {
                user.Code = null;
                user.CodeExpiry = null;
                await userManager.UpdateAsync(user);
                return Ok(localizer["confirmemail"].Value);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }
        [HttpGet("ResendCode")]
        public async Task<IActionResult> ResendCode(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) 
                return NotFound(localizer["usernotfound"].Value);

            Random generator = new Random();
            string code = generator.Next(0, 1000000).ToString("D6");
            user.Code = code;
            user.CodeExpiry = DateTime.Now.AddMinutes(5);
            await userManager.UpdateAsync(user);

            await emailService.SendEmailAsync(
                Email: email,
                subject: localizer["confirmyouremail"].Value,
                body: $"{localizer["confirmEmailCode"].Value} {code}"
                );
            return Ok($"{localizer["confirmationCodeResent"].Value} \"{email}\"");

        }
        [HttpPut("ChangeEmail")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(string newemail)
        {
            var userid= User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userid)) return Unauthorized();
            var user = await userManager.FindByIdAsync(userid);

            if (user == null) 
                return NotFound(localizer["usernotfound"].Value);

            var existingUser = await userManager.FindByEmailAsync(newemail);
            if (existingUser != null)
                return BadRequest(localizer["email"].Value);

            user.Email = newemail;

            Random generator = new Random();
            string code = generator.Next(0, 1000000).ToString("D6");
            user.Code = code;
            user.CodeExpiry = DateTime.Now.AddMinutes(5);
            user.RefrenceNewEmail = newemail;
            await userManager.UpdateAsync(user);

            await emailService.SendEmailAsync(
                Email: newemail,
                subject: localizer["confirmyouremail"].Value,
                body: $"{localizer["confirmEmailCode"].Value} {code}"
             );
            return Ok($"{localizer["confirmationCodeResent"].Value} {newemail}");
        }

        [HttpGet("ConfirmEmailChange")]
        [Authorize]
        public async Task<IActionResult> ConfirmEmailChange(string code)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userid)) return Unauthorized();
            var user = await userManager.FindByIdAsync(userid);
            if (user == null)
                return NotFound(localizer["usernotfound"].Value);

            if (user.Code != code)
                return BadRequest(localizer["invalidcode"].Value);

            if (user.CodeExpiry < DateTime.Now)
                return BadRequest(localizer["codeexpired"].Value);

            var newmail = user.RefrenceNewEmail;
            var token = await userManager.GenerateChangeEmailTokenAsync(user, newmail);
            var result = await userManager.ChangeEmailAsync(user, newmail, token);
            if(result.Succeeded)
            {
                user.Code = null;
                user.RefrenceNewEmail = null;
                user.CodeExpiry = null;
                await userManager.UpdateAsync(user);
                return Ok(localizer["emailChangeSuccess"].Value);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> changePassword(string currentPassword, string newpassword)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userid)) return Unauthorized();
            var user = await userManager.FindByIdAsync(userid);
            if (user == null)
                return NotFound(localizer["usernotfound"].Value);

            if (currentPassword == newpassword)
                return BadRequest("new password must be diffrent from current password");

            var result = await userManager.ChangePasswordAsync(user, currentPassword, newpassword);
            if (result.Succeeded)
            {
                return Ok(localizer["passwordChangedSuccessfully"].Value);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            var user=  await userManager.FindByEmailAsync(Email);
            if (user == null) return NotFound(localizer["usernotfound"].Value);

            Random generator = new Random();
            string code = generator.Next(0, 1000000).ToString("D6");
            user.Code = code;
            user.CodeExpiry = DateTime.Now.AddMinutes(5);
            await userManager.UpdateAsync(user);
            await emailService.SendEmailAsync(
                Email: Email,
                subject: localizer["resetPasswordCodeTitle"].Value,
                body: $"{localizer["resetPasswordCodeMessage"].Value} {code}"
                );
            return Ok($"{localizer["passwordResetCodeSent"].Value} {Email}");
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string Email,string code,string NewPassword)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
                return NotFound(localizer["usernotfound"].Value);
            if (user.Code != code)
                return BadRequest("Invalid code");

            if (user.CodeExpiry < DateTime.Now)
                return BadRequest("Code expired");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, NewPassword);

            if (result.Succeeded)
            {
                user.Code = null;
                user.CodeExpiry = null;
                await userManager.UpdateAsync(user);
                return Ok(localizer["passwordResetSuccess"].Value);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);

        }

    }
}
