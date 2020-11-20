using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frapper.Web.Messages
{
    public enum NotificationType
    {
        /// <summary>
        /// Success
        /// </summary>
        success,

        /// <summary>
        /// Error
        /// </summary>
        danger,

        /// <summary>
        /// Warning
        /// </summary>
        warning,

        /// <summary>
        /// Information
        /// </summary>
        info
    }
}
