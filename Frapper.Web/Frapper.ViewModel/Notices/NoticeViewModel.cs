using System;
using System.ComponentModel.DataAnnotations;

namespace Frapper.ViewModel.Notices
{
    public class CreateNoticeViewModel
    {
        [MinLength(5, ErrorMessage = "Minimum Username must be 5 in characters")]
        [Required(ErrorMessage = "Enter Notice Title")]
        [Display(Name = "NoticeTitle")]
        public string NoticeTitle { get; set; }

        [Required(ErrorMessage = "Select Notice Start")]
        [Display(Name = "NoticeStart")]
        public string NoticeStart { get; set; }

        [Required(ErrorMessage = "Select Notice End")]
        [Display(Name = "NoticeEnd")]
        public string NoticeEnd { get; set; }

        [Required(ErrorMessage = "Enter Notice Body")]
        [Display(Name = "NoticeBody")]
        public string NoticeBody { get; set; }
    }


    public class EditNoticeViewModel
    {
        public int NoticeId { get; set; }

        [MinLength(5, ErrorMessage = "Minimum Username must be 5 in characters")]
        [Required(ErrorMessage = "Enter Notice Title")]
        [Display(Name = "NoticeTitle")]
        public string NoticeTitle { get; set; }

        [Required(ErrorMessage = "Select Notice Start")]
        [Display(Name = "NoticeStart")]
        public string NoticeStart { get; set; }

        [Required(ErrorMessage = "Select Notice End")]
        [Display(Name = "NoticeEnd")]
        public string NoticeEnd { get; set; }

        [Required(ErrorMessage = "Enter Notice Body")]
        [Display(Name = "NoticeBody")]
        public string NoticeBody { get; set; }
        public bool Status { get; set; }
    }

    public class RequestDeleteNotice
    {
        public int NoticeId { get; set; }
    }
}