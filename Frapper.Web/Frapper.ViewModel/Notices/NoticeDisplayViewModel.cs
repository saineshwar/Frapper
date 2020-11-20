using System;

namespace Frapper.ViewModel.Notices
{
    public class NoticeDisplayViewModel
    {
        public int NoticeId { get; set; }
        public string NoticeTitle { get; set; }
        public DateTime? NoticeStart { get; set; }
        public DateTime? NoticeEnd { get; set; }
        public string CreatedOn { get; set; }
        public string NoticeBody { get; set; }
    }
}