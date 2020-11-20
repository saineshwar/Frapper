using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Frapper.Entities.Usermaster
{
    [Table("UserTokens")]
    public class UserTokens
    {
        [Key]
        public int HashId { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [ForeignKey("UserMaster")]
        public long UserId { get; set; }
        public UserMaster UserMaster { get; set; }
    }
}
