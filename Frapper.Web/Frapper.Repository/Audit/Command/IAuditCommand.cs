using Frapper.ViewModel.Audit;

namespace Frapper.Repository.Audit.Command
{
    public interface IAuditCommand
    {
        public void InsertAuditData(AuditTbModel objaudittb);
    }
}