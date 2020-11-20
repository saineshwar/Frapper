using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using Frapper.Common;
using Frapper.Entities.Usermaster;
using Frapper.ViewModel.Messages;
using Microsoft.Extensions.Options;

namespace Frapper.Services.Messages
{
    public class EmailSender : IEmailSender
    {
        private readonly AppSettings _appSettings;
        public EmailSender(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        } 

        public void SendMailusingSmtp(MessageTemplate messageTemplate)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            try
            {
                MailAddress mailAddress = new MailAddress(_appSettings.FromAddress, _appSettings.FromName);
                message.From = mailAddress;
                message.To.Add(messageTemplate.ToAddress);
                message.Subject = messageTemplate.Subject;
                message.IsBodyHtml = true;

                if (messageTemplate.Bcc != null)
                {
                    foreach (var address in messageTemplate.Bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                    {
                        message.Bcc.Add(address.Trim());
                    }
                }

                if (messageTemplate.Cc != null)
                {
                    foreach (var address in messageTemplate.Cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                    {
                        message.CC.Add(address.Trim());
                    }
                }

                message.Body = messageTemplate.Body;
                smtpClient.Host = _appSettings.Host;
                smtpClient.Port = Convert.ToInt32(_appSettings.Port);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(_appSettings.FromAddress, _appSettings.Password);
                smtpClient.Send(message);
                smtpClient.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string CreateVerificationEmail(UserMaster user, string token)
        {
            AesAlgorithm aesAlgorithm = new AesAlgorithm();
            var key = string.Join(":", new string[] { DateTime.Now.Ticks.ToString(), user.UserId.ToString() });
            var encrypt = aesAlgorithm.EncryptToBase64String(key);

            var linktoverify = $"{_appSettings.VerifyResetPasswordUrl}?key={HttpUtility.UrlEncode(encrypt)}&hashtoken={HttpUtility.UrlEncode(token)}";
            var stringtemplate = new StringBuilder();
            stringtemplate.Append("Welcome");
            stringtemplate.Append("<br/>");
            stringtemplate.Append($"Dear {user.FirstName}{user.LastName}");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Please click the following link to reset your password.");
            stringtemplate.Append("<br/>");
            stringtemplate.Append($"Reset password link : <a target='_blank' href={linktoverify}>Link</a>");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("If the link does not work, copy and paste the URL into a new browser window. The URL will expire in 24 hours for security reasons.");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Best regards,");
            stringtemplate.Append("Frapper");
            stringtemplate.Append("<br/>");
            return stringtemplate.ToString();
        }


        public string CreateRegistrationVerificationEmail(UserMaster user, string token)
        {
            AesAlgorithm aesAlgorithm = new AesAlgorithm();
            var key = string.Join(":", new string[] { DateTime.Now.Ticks.ToString(), user.UserId.ToString() });
            var encrypt = aesAlgorithm.EncryptToBase64String(key);

            var linktoverify = $"{_appSettings.VerifyRegistrationUrl}?key={HttpUtility.UrlEncode(encrypt)}&hashtoken={HttpUtility.UrlEncode(token)}";
            var stringtemplate = new StringBuilder();
            stringtemplate.Append("Welcome");
            stringtemplate.Append("<br/>");
            stringtemplate.Append($"Dear {user.FirstName}{user.LastName}");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Thanks for joining Web Secure.");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("To activate your Web Secure account, please confirm your email address.");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("<a target='_blank' href=" + linktoverify + ">Confirm Email</a>");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Yours sincerely,");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Frapper");
            stringtemplate.Append("<br/>");
            return stringtemplate.ToString();
        }
    }
}
