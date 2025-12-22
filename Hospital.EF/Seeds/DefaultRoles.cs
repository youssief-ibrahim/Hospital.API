using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Hospital.EF.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            var roles = new List<string> { "Admin", "Patient" , "Doctor" , "Staff" , "Accountant" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name=role,
                        NormalizedName = role.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    });
                }
            }
        }
    }
}
