using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Frapper.Entities.Menus
{
    [Table("MenuMaster")]
    public class MenuMaster
    {
        [Key]
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }

        [ForeignKey("MenuCategory")]
        public int? MenuCategoryId { get; set; }
        public int? RoleId { get; set; }
        public int? SortingOrder { get; set; }
    }
}
