using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frapper.Entities.Usermaster;
using Frapper.ViewModel.Usermaster;

namespace Frapper.Repository.Usermaster.Queries
{
    public interface IUserMasterQueries
    {
        IQueryable<UserMasterGrid> ShowAllUsers(string sortColumn, string sortColumnDir, string search);
        bool CheckUserExists(string username);
        UserMaster GetUserbyUserName(string username);
        CommonUserDetailsViewModel GetCommonUserDetailsbyUserName(string username);
        bool CheckEmailExists(string emailid);
        bool CheckMobileNoExists(string mobileno);
        EditUserViewModel GetUserForEditByUserId(long? userId);
        UserMaster GetUserDetailsbyUserId(long? userId);
        bool CheckIsAlreadyVerifiedRegistration(long userid);
        EditUserProfileViewModel GetProfileForEditByUserId(long? userId);
    }
}
