using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Frapper.Web.Messages
{
    public class NotificationService : INotificationService
    {
        public static string NotificationListKey => "Notification";

        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NotificationService(ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContextAccessor)
        {
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Notification(string alertTitle, NotificationType type, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.info, message, encode);
        }

        public void SuccessNotification(string alertTitle, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.success, message, encode);
        }

        public void WarningNotification(string alertTitle, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.warning, message, encode);
        }

        public void DangerNotification(string alertTitle, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.danger, message, encode);
        }

        protected void PrepareTempData(string alertTitle, NotificationType type, string message, bool encode = true)
        {
            var context = _httpContextAccessor.HttpContext;
            var tempData = _tempDataDictionaryFactory.GetTempData(context);

            //Messages have stored in a serialized list
            var messages = tempData.ContainsKey(NotificationListKey)
                ? JsonConvert.DeserializeObject<IList<NotificationData>>(tempData[NotificationListKey].ToString() ?? string.Empty)
                : new List<NotificationData>();

            messages.Add(new NotificationData
            {
                Message = message,
                Type = type,
                Encode = encode,
                AlertTitle = alertTitle
            });

            tempData[NotificationListKey] = JsonConvert.SerializeObject(messages);
        }

        
    }
}
