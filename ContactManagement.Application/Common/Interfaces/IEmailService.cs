using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendWelcomeEmailAsync(string to, string userName);
        Task SendPasswordResetEmailAsync(string to, string resetToken);
        Task SendContactCreatedEmailAsync(string to, string contactName);
    }
}
