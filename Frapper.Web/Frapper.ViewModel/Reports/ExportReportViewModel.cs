using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frapper.ViewModel.Reports
{
    public class ExportReportViewModel
    {
        [Display(Name = "Fromdate")]
        [Required(ErrorMessage = "Required From-date")]
        public string Fromdate { get; set; }

        [Display(Name = "Todate")]
        [Required(ErrorMessage = "Required To-date")]
        public string Todate { get; set; }
        public List<SelectListItem> ListofReportType { get; set; }

        [Display(Name = "ReportType")]
        [Required(ErrorMessage = "Required Report Type")]
        public int ReportTypeId { get; set; }

        public List<SelectListItem> ListofReports{ get; set; }

        [Display(Name = "Report")]
        [Required(ErrorMessage = "Required Report")]
        public int ReportsId { get; set; }
    }
}