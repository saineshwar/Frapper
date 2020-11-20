

using Frapper.Repository.Documents.Command;
using Frapper.Repository.EmailVerification.Command;
using Frapper.Repository.Menus.Command;
using Frapper.Repository.Notices.Command;
using Frapper.Repository.ProfileImage.Command;
using Frapper.Repository.Rolemasters.Command;
using Frapper.Repository.Usermaster.Command;

namespace Frapper.Repository
{
    public interface IUnitOfWorkEntityFramework
    {
        IRoleCommand RoleCommand { get; }
        IUserMasterCommand UserMasterCommand { get; }
        IAssignedRolesCommand AssignedRolesCommand { get; }
        IUserTokensCommand UserTokensCommand { get; }
        IMenuCategoryCommand MenuCategoryCommand { get; }
        IMenuMasterCommand MenuMasterCommand { get; }
        ISubMenuMasterCommand SubMenuMasterCommand { get; } 
        IVerificationCommand VerificationCommand { get; }
        IProfileImageCommand ProfileImageCommand { get; }
        INoticeCommand NoticeCommand { get; }
        INoticeDetailsCommand NoticeDetailsCommand { get; }
        IDocumentCommand DocumentCommand { get; }
        bool Commit();
        void Dispose();
    }
}