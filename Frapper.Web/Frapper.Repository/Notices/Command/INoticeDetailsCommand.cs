using Frapper.Entities.Notices;

namespace Frapper.Repository.Notices.Command
{
    public interface INoticeDetailsCommand
    {
        void AddNoticeDetails(NoticeDetails noticeDetails);
        void UpdateNoticeDetails(NoticeDetails noticeDetails);
        void DeleteNoticeDetails(NoticeDetails noticeDetails);
    }
}