using Frapper.Entities.Menus;
using Frapper.Entities.ProfileImage;
using Microsoft.EntityFrameworkCore;

namespace Frapper.Repository.ProfileImage.Command
{
    public class ProfileImageCommand : IProfileImageCommand
    {
        private readonly FrapperDbContext _frapperDbContext;
        public ProfileImageCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }

        public void Add(ProfileImageProperty profileImage)
        {
            _frapperDbContext.ProfileImagePropertys.Add(profileImage);
        }

        public void Update(ProfileImageProperty profileImage)
        {
            _frapperDbContext.Entry(profileImage).State = EntityState.Modified;
        }
    }
}