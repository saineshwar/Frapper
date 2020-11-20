using Frapper.Entities.Usermaster;
using Frapper.Entities.Verification;

namespace Frapper.Repository.EmailVerification.Command
{
    public interface IVerificationCommand
    {
        void SendRegistrationVerificationToken(long userid, string verficationToken);
        void SendResetVerificationToken(long userid, string verficationToken);
        void UpdateRegisterVerification(RegisterVerification registerVerification);
        void UpdateResetVerification(ResetPasswordVerification resetPasswordVerification);
    }
}