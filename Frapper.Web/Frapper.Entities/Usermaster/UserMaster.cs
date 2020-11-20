using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;


namespace Frapper.Entities.Usermaster
{
    [Table("Usermaster")]
    public class UserMaster
    {
        [Key]
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public bool Status { get; set; }
        public long? CreatedBy { get; set; }
        public bool IsFirstLogin { get; set; } = false;
        public DateTime? ModifiedOn { get; set; }
        public AssignedRoles AssignedRoles { get; set; }
        public UserTokens UserTokens { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
