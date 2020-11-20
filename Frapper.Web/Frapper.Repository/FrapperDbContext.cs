using System;
using System.Collections.Generic;
using System.Text;
using Frapper.Entities.Documents;
using Frapper.Entities.Menus;
using Frapper.Entities.Notices;
using Frapper.Entities.ProfileImage;
using Frapper.Entities.Rolemasters;
using Frapper.Entities.Usermaster;
using Frapper.Entities.Verification;
using Microsoft.EntityFrameworkCore;

namespace Frapper.Repository
{
    public class FrapperDbContext
        : DbContext
    {
        public FrapperDbContext(DbContextOptions<FrapperDbContext> options) : base(options)
        {

        }

        public DbSet<UserMaster> UserMasters { get; set; }
        public DbSet<AssignedRoles> AssignedRoles { get; set; }
        public DbSet<UserTokens> UserTokens { get; set; }
        public DbSet<RoleMaster> RoleMasters { get; set; }
        public DbSet<RegisterVerification> RegisterVerification { get; set; }
        public DbSet<MenuCategory> MenuCategorys { get; set; }
        public DbSet<MenuMaster> MenuMasters { get; set; }
        public DbSet<SubMenuMaster> SubMenuMasters { get; set; }
        public DbSet<ResetPasswordVerification> ResetPasswordVerification { get; set; }
        public DbSet<ProfileImageProperty> ProfileImagePropertys { get; set; }
        public DbSet<Notice> Notice { get; set; }
        public DbSet<NoticeDetails> NoticeDetails { get; set; }
        public DbSet<DocumentUploadedFiles> DocumentUploadedFiles { get; set; }
    }
}