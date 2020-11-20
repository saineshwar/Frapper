using System;
using System.Linq;
using Frapper.Entities.Rolemasters;
using System.Linq.Dynamic.Core;
using Frapper.Entities.Usermaster;
using Frapper.ViewModel.Usermaster;


namespace Frapper.Repository.Usermaster.Queries
{
    public class UserMasterQueries : IUserMasterQueries
    {
        private readonly FrapperDbContext _frapperDbContext;
        public UserMasterQueries(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }
        public IQueryable<UserMasterGrid> ShowAllUsers(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesUserMasters = (from userMaster in _frapperDbContext.UserMasters
                                             join assignedRole in _frapperDbContext.AssignedRoles on userMaster.UserId equals assignedRole.UserId
                                             join roles in _frapperDbContext.RoleMasters on assignedRole.RoleId equals roles.RoleId
                                             select new UserMasterGrid()
                                             {
                                                 CreatedOn = userMaster.CreatedOn,
                                                 EmailId = userMaster.EmailId,
                                                 FirstName = string.IsNullOrEmpty(userMaster.FirstName) ? "-" : userMaster.FirstName,
                                                 Gender = string.IsNullOrEmpty(userMaster.Gender) ? "-" : userMaster.Gender == "M" ? "Male" : "Female",
                                                 LastName = string.IsNullOrEmpty(userMaster.LastName) ? "-" : userMaster.LastName,
                                                 MobileNo = userMaster.MobileNo,
                                                 RoleName = roles.RoleName,
                                                 UserId = userMaster.UserId,
                                                 UserName = userMaster.UserName,
                                                 Status = userMaster.Status == true ? "Active" : "InActive"
                                             }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesUserMasters = queryablesUserMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryablesUserMasters = queryablesUserMasters.Where(m => m.UserName.Contains(search) || m.UserName.Contains(search));
                }

                return queryablesUserMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool CheckUserExists(string username)
        {
            var userdata = (from tempuser in _frapperDbContext.UserMasters
                            where tempuser.UserName == username
                            select tempuser).Any();

            return userdata;
        }
        public bool CheckEmailExists(string emailid)
        {
            var userdata = (from tempuser in _frapperDbContext.UserMasters
                            where tempuser.EmailId == emailid
                            select tempuser).Any();

            return userdata;
        }
        public bool CheckMobileNoExists(string mobileno)
        {
            var userdata = (from tempuser in _frapperDbContext.UserMasters
                            where tempuser.MobileNo == mobileno
                            select tempuser).Any();

            return userdata;
        }
        public UserMaster GetUserbyUserName(string username)
        {
            var userdata = (from tempuser in _frapperDbContext.UserMasters
                            where tempuser.UserName == username
                            select tempuser).FirstOrDefault();

            return userdata;
        }

        public CommonUserDetailsViewModel GetCommonUserDetailsbyUserName(string username)
        {
            var userdata = (from tempuser in _frapperDbContext.UserMasters
                            join assignedRoles in _frapperDbContext.AssignedRoles on tempuser.UserId equals assignedRoles.UserId
                            join roleMaster in _frapperDbContext.RoleMasters on assignedRoles.RoleId equals roleMaster.RoleId
                            where tempuser.UserName == username
                            select new CommonUserDetailsViewModel()
                            {
                                FirstName = tempuser.FirstName,
                                EmailId = tempuser.EmailId,
                                LastName = tempuser.LastName,
                                RoleId = roleMaster.RoleId,
                                UserId = tempuser.UserId,
                                RoleName = roleMaster.RoleName,
                                Status = tempuser.Status,
                                UserName = tempuser.UserName,
                                PasswordHash = tempuser.PasswordHash,
                                MobileNo = tempuser.MobileNo
                            }).FirstOrDefault();

            return userdata;
        }

        public EditUserViewModel GetUserForEditByUserId(long? userId)
        {
            var role = (from tempuser in _frapperDbContext.UserMasters
                        join assignedRole in _frapperDbContext.AssignedRoles on tempuser.UserId equals assignedRole.UserId
                        join roles in _frapperDbContext.RoleMasters on assignedRole.RoleId equals roles.RoleId
                        where tempuser.UserId == userId
                        select new EditUserViewModel()
                        {
                            FirstName = tempuser.FirstName,
                            EmailId = tempuser.EmailId,
                            LastName = tempuser.LastName,
                            MobileNo = tempuser.MobileNo,
                            Gender = tempuser.Gender,
                            RoleId = roles.RoleId,
                            Status = roles.Status,
                            UserName = tempuser.UserName,
                            UserId = tempuser.UserId
                        }).FirstOrDefault();
            return role;
        }

        public EditUserProfileViewModel GetProfileForEditByUserId(long? userId)
        {
            var role = (from tempuser in _frapperDbContext.UserMasters
                join assignedRole in _frapperDbContext.AssignedRoles on tempuser.UserId equals assignedRole.UserId
                join roles in _frapperDbContext.RoleMasters on assignedRole.RoleId equals roles.RoleId
                where tempuser.UserId == userId
                select new EditUserProfileViewModel()
                {
                    FirstName = tempuser.FirstName,
                    EmailId = tempuser.EmailId,
                    LastName = tempuser.LastName,
                    MobileNo = tempuser.MobileNo,
                    Gender = tempuser.Gender,
                    UserId = tempuser.UserId
                }).FirstOrDefault();
            return role;
        }

        public UserMaster GetUserDetailsbyUserId(long? userId)
        {
            var userdata = (from tempuser in _frapperDbContext.UserMasters
                            where tempuser.UserId == userId
                            select tempuser).FirstOrDefault();

            return userdata;
        }
        public bool CheckIsAlreadyVerifiedRegistration(long userid)
        {
            var registerVerification = (from rv in _frapperDbContext.RegisterVerification
                                        where rv.UserId == userid && rv.VerificationStatus == true
                                        select rv).Any();

            return registerVerification;
        }

        
    }
}