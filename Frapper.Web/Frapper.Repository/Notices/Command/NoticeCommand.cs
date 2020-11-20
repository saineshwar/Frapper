using Frapper.Entities.Notices;
using Frapper.Entities.Usermaster;
using Microsoft.EntityFrameworkCore;

namespace Frapper.Repository.Notices.Command
{
    public class NoticeCommand : INoticeCommand
    {
        private readonly FrapperDbContext _frapperDbContext;
        public NoticeCommand(FrapperDbContext frapperDbContext)
        {
            _frapperDbContext = frapperDbContext;
        }

        public void AddNotice(Notice notice)
        {
            _frapperDbContext.Notice.Add(notice);
        }

        public void UpdateNotice(Notice notice)
        {
            _frapperDbContext.Entry(notice).State = EntityState.Modified;
        }

        public void DeleteNotice(Notice notice)
        {
            _frapperDbContext.Entry(notice).State = EntityState.Deleted;
        }
    }
}