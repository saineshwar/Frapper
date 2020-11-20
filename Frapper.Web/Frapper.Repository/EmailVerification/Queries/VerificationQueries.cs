using System;
using System.Linq.Dynamic.Core;
using Frapper.Entities.Usermaster;
using Frapper.Entities.Verification;
using System.Linq;

namespace Frapper.Repository.EmailVerification.Queries
{
    public class VerificationQueries : IVerificationQueries
    {
        private readonly FrapperDbContext _frapperDbContext;
        public VerificationQueries(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }
        public bool CheckIsAlreadyVerifiedRegistration(long userid)
        {
            var registerVerification = (from rv in _frapperDbContext.RegisterVerification
                                        where rv.UserId == userid && rv.VerificationStatus == true
                                        select rv).Any();

            return registerVerification;
        }

        public RegisterVerification GetRegistrationGeneratedToken(long userid)
        {
            var registerVerification = (from rv in _frapperDbContext.RegisterVerification
                                        orderby rv.RegisterVerificationId descending
                                        where rv.UserId == userid
                                        select rv).FirstOrDefault();

            return registerVerification;
        }

        public ResetPasswordVerification GetResetGeneratedToken(long userid)
        {
            var resetPasswordVerification = (from rv in _frapperDbContext.ResetPasswordVerification
                                             orderby rv.ResetTokenId descending
                                             where rv.UserId == userid
                                             select rv).FirstOrDefault();

            return resetPasswordVerification;
        }
    }
}