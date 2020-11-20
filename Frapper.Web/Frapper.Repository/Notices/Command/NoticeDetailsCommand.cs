using Frapper.Entities.Notices;
using Frapper.Entities.Usermaster;
using Microsoft.EntityFrameworkCore;

namespace Frapper.Repository.Notices.Command
{
    public class NoticeDetailsCommand : INoticeDetailsCommand
    {
        private readonly FrapperDbContext _frapperDbContext;
        public NoticeDetailsCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }

        public void AddNoticeDetails(NoticeDetails noticeDetails)
        {
            _frapperDbContext.NoticeDetails.Add(noticeDetails);
        }

        public void UpdateNoticeDetails(NoticeDetails noticeDetails)
        {
            _frapperDbContext.Entry(noticeDetails).State = EntityState.Modified;
        }

        public void DeleteNoticeDetails(NoticeDetails noticeDetails)
        {
            _frapperDbContext.Entry(noticeDetails).State = EntityState.Deleted;
        }
    }
}