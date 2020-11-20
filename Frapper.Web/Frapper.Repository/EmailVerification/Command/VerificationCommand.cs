using System;
using Frapper.Entities.Usermaster;
using Frapper.Entities.Verification;

namespace Frapper.Repository.EmailVerification.Command
{
    public class VerificationCommand : IVerificationCommand
    {
        private readonly FrapperDbContext _frapperDbContext;
        public VerificationCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }
        public void SendRegistrationVerificationToken(long userid, string verficationToken)
        {
            RegisterVerification registerVerification = new RegisterVerification()
            {
                RegisterVerificationId = 0,
                GeneratedDate = DateTime.Now,
                GeneratedToken = verficationToken,
                UserId = userid,
                Status = true,
                VerificationStatus = false
            };

            _frapperDbContext.RegisterVerification.Add(registerVerification);
        }

        public void SendResetVerificationToken(long userid, string verficationToken)
        {
            ResetPasswordVerification registerVerification = new ResetPasswordVerification()
            {
                ResetTokenId = 0,
                GeneratedDate = DateTime.Now,
                GeneratedToken = verficationToken,
                UserId = userid,
                Status = true,
                VerificationStatus = false
            };

            _frapperDbContext.ResetPasswordVerification.Add(registerVerification);
        }

        public void UpdateRegisterVerification(RegisterVerification registerVerification)
        {
            registerVerification.VerificationStatus = true;
            registerVerification.VerificationDate = DateTime.Now;
            _frapperDbContext.RegisterVerification.Update(registerVerification);
           
        }

        public void UpdateResetVerification(ResetPasswordVerification resetPasswordVerification)
        {
            resetPasswordVerification.VerificationStatus = true;
            resetPasswordVerification.VerificationDate = DateTime.Now;
            _frapperDbContext.ResetPasswordVerification.Update(resetPasswordVerification);
        }
    }
}