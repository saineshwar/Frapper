using System;

namespace Frapper.ViewModel.Notices
{
    public class NoticeGrid
    {
        public int NoticeId { get; set; }
        public string NoticeTitle { get; set; }
        public DateTime? NoticeStart { get; set; }
        public DateTime? NoticeEnd { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Status { get; set; }
    }
}