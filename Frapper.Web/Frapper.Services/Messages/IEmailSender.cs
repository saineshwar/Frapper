using System.Collections.Generic;
using Frapper.Entities.Usermaster;
using Frapper.ViewModel.Messages;

namespace Frapper.Services.Messages
{
    public interface IEmailSender
    {
        void SendMailusingSmtp(MessageTemplate messageTemplate);
        string CreateVerificationEmail(UserMaster user, string token);
        string CreateRegistrationVerificationEmail(UserMaster user, string token);
    }
}