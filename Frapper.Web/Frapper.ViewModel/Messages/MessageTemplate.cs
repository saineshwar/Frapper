using System.Collections.Generic;

namespace Frapper.ViewModel.Messages
{
    public class MessageTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ToAddress { get; set; }
        public IEnumerable<string> Bcc { get; set; }
        public IEnumerable<string> Cc { get; set; }
    }
}