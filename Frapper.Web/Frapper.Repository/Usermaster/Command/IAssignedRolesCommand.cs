using Frapper.Entities.Usermaster;

namespace Frapper.Repository.Usermaster.Command
{
    public interface IAssignedRolesCommand
    {
        void Add(AssignedRoles assignedRoles);
        void Update(AssignedRoles assignedRoles);
    }
}