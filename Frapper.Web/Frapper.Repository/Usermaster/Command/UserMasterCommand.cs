using System;
using System.Collections.Generic;
using Frapper.Entities.Usermaster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace Frapper.Repository.Usermaster.Command
{
    public class UserMasterCommand : IUserMasterCommand
    {
        private readonly FrapperDbContext _frapperDbContext;

        public UserMasterCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }

        public void Add(UserMaster usermaster)
        {
            _frapperDbContext.UserMasters.Add(usermaster);
        }

        public void Update(UserMaster usermaster)
        {
            _frapperDbContext.Entry(usermaster).State = EntityState.Modified;
        }

        public void ChangeUserStatus(UserMaster usermaster)
        {
            _frapperDbContext.Entry(usermaster).State = EntityState.Modified;
        }

        public void UpdatePasswordandHistory(long userId, string passwordHash, string passwordSalt, string processType)
        {
            var parameters = new List<SqlParameter>() {
                new SqlParameter
                {
                    ParameterName = "@UserId", Value=userId
                },
                new SqlParameter
                {
                    ParameterName = "@PasswordHash", Value=passwordHash
                }, new SqlParameter
                {
                    ParameterName = "@PasswordSalt", Value=passwordSalt
                }, new SqlParameter
                {
                    ParameterName = "@ProcessType", Value=processType
                }
            };

            _frapperDbContext.Database.ExecuteSqlRaw("EXECUTE Usp_UpdatePassword @UserId,@PasswordHash,@PasswordSalt,@ProcessType",
                parameters);
        }
    }
}