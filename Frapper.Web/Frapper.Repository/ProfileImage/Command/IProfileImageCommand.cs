using Frapper.Entities.ProfileImage;

namespace Frapper.Repository.ProfileImage.Command
{
    public interface IProfileImageCommand
    {
        void Add(ProfileImageProperty profileImage);

        void Update(ProfileImageProperty profileImage);
    }
}