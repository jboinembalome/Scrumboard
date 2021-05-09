using Scrumboard.Application.Models.Mail;
using System.Threading.Tasks;

namespace Scrumboard.Application.Interfaces.Mail
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
