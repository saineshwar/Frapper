using System;
using System.Linq;
using Frapper.Entities.Rolemasters;
using System.Linq.Dynamic.Core;
using Frapper.Entities.Usermaster;

namespace Frapper.Repository.Usermaster.Queries
{
    public class AssignedRolesQueries : IAssignedRolesQueries
    {
        private readonly FrapperDbContext _frapperDbContext;
        public AssignedRolesQueries(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }
        public AssignedRoles GetAssignedRolesDetailsbyUserId(long? userId)
        {
            var assignedRoles = (from tempAssignedRole in _frapperDbContext.AssignedRoles
                                 where tempAssignedRole.UserId == userId
                                 select tempAssignedRole).FirstOrDefault();
            return assignedRoles;
        }
    }
}
