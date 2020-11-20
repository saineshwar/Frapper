using Frapper.Entities.Usermaster;
using Frapper.Entities.Verification;

namespace Frapper.Repository.EmailVerification.Queries
{
    public interface IVerificationQueries
    {
        RegisterVerification GetRegistrationGeneratedToken(long userid);
        ResetPasswordVerification GetResetGeneratedToken(long userid);
        bool CheckIsAlreadyVerifiedRegistration(long userid);
    }
}