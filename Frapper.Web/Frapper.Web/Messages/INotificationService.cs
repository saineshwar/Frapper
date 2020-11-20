using System;

namespace Frapper.Web.Messages
{
    public partial interface INotificationService
    {
        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="alertTitle"></param>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="encode">A value indicating whether the message should not be encoded</param>
        void Notification(string alertTitle, NotificationType type, string message, bool encode = true);

        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="alertTitle"></param>
        /// <param name="message">Message</param>
        /// <param name="encode">A value indicating whether the message should not be encoded</param>
        void SuccessNotification(string alertTitle, string message, bool encode = true);

        /// <summary>
        /// Display warning notification
        /// </summary>
        /// <param name="alertTitle"></param>
        /// <param name="message">Message</param>
        /// <param name="encode">A value indicating whether the message should not be encoded</param>
        void WarningNotification(string alertTitle, string message, bool encode = true);

        /// <summary>
        /// Display Error notification
        /// </summary>
        /// <param name="alertTitle"></param>
        /// <param name="message"></param>
        /// <param name="encode"></param>
        void DangerNotification(string alertTitle, string message, bool encode = true);
    }
}