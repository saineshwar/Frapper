using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frapper.Entities.Rolemasters
{
    [Table("RoleMaster")]
    public class RoleMaster
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
    }
}