using Frapper.Entities.Notices;

namespace Frapper.Repository.Notices.Command
{
    public interface INoticeCommand
    {
        void AddNotice(Notice notice);
        void UpdateNotice(Notice notice);
        void DeleteNotice(Notice notice);
    }
}