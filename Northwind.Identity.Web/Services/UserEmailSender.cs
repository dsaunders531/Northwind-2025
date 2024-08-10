using Microsoft.AspNetCore.Identity;
using Northwind.Identity.Web.Models;

namespace Northwind.Identity.Web.Services
{
    public class UserEmailSender : IEmailSender<ApplicationUser>
    {
        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            // TODO
            // HINT - instead of blocking while sending a message, put the message in a queue instead.
            // Then another process (or event handler) can send the message.
            // This is better for site performance as there are no blocking calls this way.
            throw new NotImplementedException();
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
