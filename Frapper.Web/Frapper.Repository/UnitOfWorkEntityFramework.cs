using System;
using Frapper.Repository.Documents.Command;
using Frapper.Repository.EmailVerification.Command;
using Frapper.Repository.Menus.Command;
using Frapper.Repository.Notices.Command;
using Frapper.Repository.ProfileImage.Command;
using Frapper.Repository.Rolemasters.Command;
using Frapper.Repository.Usermaster.Command;


namespace Frapper.Repository
{
    public class UnitOfWorkEntityFramework : IUnitOfWorkEntityFramework
    {
        private readonly FrapperDbContext _context;
        public IRoleCommand RoleCommand { get; }
        public IUserMasterCommand UserMasterCommand { get; }
        public IAssignedRolesCommand AssignedRolesCommand { get; }
        public IUserTokensCommand UserTokensCommand { get; }
        public IMenuCategoryCommand MenuCategoryCommand { get; }
        public IMenuMasterCommand MenuMasterCommand { get; }
        public ISubMenuMasterCommand SubMenuMasterCommand { get; }
        public IVerificationCommand VerificationCommand { get; }
        public IProfileImageCommand ProfileImageCommand { get; }
        public INoticeCommand NoticeCommand { get; }
        public INoticeDetailsCommand NoticeDetailsCommand { get; }
        public IDocumentCommand DocumentCommand { get; }

        public UnitOfWorkEntityFramework(FrapperDbContext context)
        {
            _context = context;
            AssignedRolesCommand = new AssignedRolesCommand(_context);
            RoleCommand = new RoleCommand(_context);
            UserMasterCommand = new UserMasterCommand(_context);
            UserTokensCommand = new UserTokensCommand(_context);
            MenuCategoryCommand = new MenuCategoryCommand(_context);
            MenuMasterCommand = new MenuMasterCommand(_context);
            SubMenuMasterCommand = new SubMenuMasterCommand(_context);
            VerificationCommand = new VerificationCommand(_context);
            ProfileImageCommand = new ProfileImageCommand(_context);
            NoticeCommand = new NoticeCommand(_context);
            NoticeDetailsCommand = new NoticeDetailsCommand(_context);
            DocumentCommand = new DocumentCommand(_context);
        }

        public bool Commit()
        {
            bool returnValue = true;
            using var dbContextTransaction = _context.Database.BeginTransaction();
            try
            {
                _context.SaveChanges();
                dbContextTransaction.Commit();
            }
            catch (Exception ex)
            {
                //Log Exception Handling message                      
                returnValue = false;
                dbContextTransaction.Rollback();
            }

            return returnValue;
        }

        public void Dispose()
        {
            Dispose(true);
        }


        private bool _disposedValue = false; // To detect redundant calls  

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                _context.Dispose();
            }

            _disposedValue = true;
        }
    }
}