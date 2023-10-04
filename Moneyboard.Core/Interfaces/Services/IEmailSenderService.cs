using Moneyboard.Core.Helpers.Mails;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(MailRequest mailRequest);

        Task<Response> SendEmailAsync(SendGridMessage message);

        Task SendManyMailsAsync<T>(MailingRequest<T> mailing) where T : class, new();
    }
}
