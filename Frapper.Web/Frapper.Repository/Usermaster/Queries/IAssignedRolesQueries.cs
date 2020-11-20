using Frapper.Entities.Usermaster;

namespace Frapper.Repository.Usermaster.Queries
{
    public interface IAssignedRolesQueries
    {
        AssignedRoles GetAssignedRolesDetailsbyUserId(long? userId);
    }
}