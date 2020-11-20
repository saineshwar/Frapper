using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frapper.Entities.Notices
{
    [Table("Notice")]
    public class Notice
    {
        [Key]
        public int NoticeId { get; set; }
        public string NoticeTitle { get; set; }
        public DateTime NoticeStart { get; set; }
        public DateTime NoticeEnd { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public NoticeDetails NoticeDetails { get; set; }
        public bool Status { get; set; }
    }

}