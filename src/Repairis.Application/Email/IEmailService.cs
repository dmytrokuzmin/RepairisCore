using System.Threading.Tasks;

namespace Repairis.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string message);
    }
}
