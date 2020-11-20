using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frapper.Web.Messages
{
    public class NotificationData
    {
        /// <summary>
        /// Message type (success/warning/error)
        /// </summary>
        public NotificationType Type { get; set; }

        /// <summary>
        /// Message text
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Get a sets a value indicating whether the message should not be HTML encoded
        /// </summary>
        public bool Encode { get; set; }

        /// <summary>
        /// Message text
        /// </summary>
        public string AlertTitle { get; set; }
    }
}
