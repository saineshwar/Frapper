using Frapper.Entities.Usermaster;

namespace Frapper.Repository.Usermaster.Command
{
    public interface IUserMasterCommand
    {
       void Add(UserMaster usermaster);
       void Update(UserMaster usermaster);
       void ChangeUserStatus(UserMaster usermaster);
       void UpdatePasswordandHistory(long userId, string passwordHash, string passwordSalt, string processType);
    }
}