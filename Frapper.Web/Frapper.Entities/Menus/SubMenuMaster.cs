using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Frapper.Entities.Menus
{
    [Table("SubMenuMaster")]
    public class SubMenuMaster
    {
        [Key]
        public int SubMenuId { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string SubMenuName { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }

        [ForeignKey("MenuMaster")]
        public int MenuId { get; set; }
        [ForeignKey("MenuCategory")]
        public int? MenuCategoryId { get; set; }
        public int? RoleId { get; set; }
        public int? SortingOrder { get; set; }
    }
}
