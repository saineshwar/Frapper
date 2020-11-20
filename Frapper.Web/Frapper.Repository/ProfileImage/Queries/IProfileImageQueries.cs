using Frapper.Entities.ProfileImage;

namespace Frapper.Repository.ProfileImage.Queries
{
    public interface IProfileImageQueries
    {
        ProfileImageProperty GetProfileImageByProfileImageId(long userId);
        bool CheckProfileImageExists(long userId);
    }
}