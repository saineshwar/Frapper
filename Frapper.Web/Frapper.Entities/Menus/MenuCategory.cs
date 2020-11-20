using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Frapper.Entities.Menus
{
    [Table("MenuCategory")]
    public class MenuCategory
    {
        [Key]
        public int MenuCategoryId { get; set; }
        public string MenuCategoryName { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public int? SortingOrder { get; set; }

    }
}
