using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frapper.Web.Helpers
{
    public class AlertViewModel
    {
        public string AlertType { get; set; }
        public string AlertTitle { get; set; }
        public string AlertMessage { get; set; }

        public AlertViewModel(string type, string title, string message)
        {
            AlertType = type;
            AlertTitle = title;
            AlertMessage = message;
        }
    }
}
