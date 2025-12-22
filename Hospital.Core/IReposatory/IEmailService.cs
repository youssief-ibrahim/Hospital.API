using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.IReposatory
{
    public interface IEmailService
    {
        Task SendEmailAsync(string Email, string subject, string body);
    }
}
