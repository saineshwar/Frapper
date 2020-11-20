using Frapper.Entities.Usermaster;

namespace Frapper.Repository.Usermaster.Queries
{
    public interface IUserTokensQueries
    {
        UserTokens GetUserSaltbyUserid(long userId);
    }
}