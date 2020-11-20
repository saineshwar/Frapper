using Frapper.Entities.Usermaster;

namespace Frapper.Repository.Usermaster.Command
{
    public interface IUserTokensCommand
    {
        void Add(UserTokens userTokens);
    }
}