using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Frapper.Entities.Usermaster
{
    [Table("AssignedRoles")]
    public class AssignedRoles
    {
        [Key]
        public short AssignedRoleId { get; set; }
        public int RoleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
        [ForeignKey("UserMaster")]
        public long UserId { get; set; }
        public UserMaster UserMaster { get; set; }
    }

}
