using Frapper.Entities.Usermaster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frapper.Repository.Usermaster.Command
{
    public class UserTokensCommand : IUserTokensCommand
    {
        private readonly FrapperDbContext _frapperDbContext;
        public UserTokensCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }

        public void Add(UserTokens userTokens)
        {
            _frapperDbContext.UserTokens.Add(userTokens);
        }
    }
}
