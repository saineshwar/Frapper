using System;

namespace Frapper.ViewModel.Audit
{
    public class AuditTbModel
    {
        public int AuditId { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string IpAddress { get; set; }
        public DateTime? LoggedInAt { get; set; }
        public DateTime? LoggedOutAt { get; set; }
        public string LoginStatus { get; set; }
        public string PageAccessed { get; set; }
        public string SessionId { get; set; }
        public string UrlReferrer { get; set; }
        public string UserId { get; set; }
    }
}