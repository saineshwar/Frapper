using System.Collections.Generic;
using System.Linq;
using Frapper.Entities.Notices;
using Frapper.ViewModel.Notices;
using Frapper.ViewModel.Reports;

namespace Frapper.Repository.Notices.Queries
{
    public interface INoticeQueries
    {
        NoticeDisplayViewModel ShowNotice();
        bool ShowNotice(string fromdatetime, string todatetime);
        IQueryable<NoticeGrid> ShowAllNotice(string sortColumn, string sortColumnDir, string search);
        EditNoticeViewModel GetNoticeDetailsForEdit(int? noticeId);
        Notice GetNoticeByNoticeId(int? noticeId);
        NoticeDetails GetNoticeDetailsByNoticeId(int? noticeId);
    }
}