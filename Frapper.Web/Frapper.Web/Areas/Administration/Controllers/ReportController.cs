using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Services.ExcelService;
using Frapper.ViewModel.Reports;
using Frapper.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frapper.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [AuthorizeSuperAdmin]
    public class ReportController : Controller
    {
        private readonly IExcelGenerator _excelGenerator;
        public ReportController(IExcelGenerator excelGenerator)
        {
            _excelGenerator = excelGenerator;
        }
        public IActionResult Export()
        {
            var exportReportViewModel = new ExportReportViewModel
            {
                ListofReportType = ReportTypeList(),
                ListofReports = ReportsList()
            };
            return View(exportReportViewModel);
        }


        [HttpPost]
        public IActionResult Export(ExportReportViewModel exportReportViewModel)
        {
            if (ModelState.IsValid)
            {
                if (exportReportViewModel.ReportsId == 1 && exportReportViewModel.ReportTypeId == 1)
                {
                    var results = _excelGenerator.SummaryDownloadReport(exportReportViewModel.Fromdate,
                          exportReportViewModel.Todate);
                    return File(results.ExcelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", results.ExcelName);

                }

                if (exportReportViewModel.ReportsId == 1 && exportReportViewModel.ReportTypeId == 2)
                {
                    var results = _excelGenerator.DetailsDownloadReport(exportReportViewModel.Fromdate,
                       exportReportViewModel.Todate);
                    return File(results.ExcelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", results.ExcelName);
                }
            }

            exportReportViewModel.ListofReportType = ReportTypeList();
            exportReportViewModel.ListofReports = ReportsList();
            return View(exportReportViewModel);


        }


        private List<SelectListItem> ReportTypeList()
        {
            List<SelectListItem> listofReport = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "---Select---",Value = "",
                },
                new SelectListItem()
                {
                    Text = "Summary Report",Value = "1",
                },
                new SelectListItem()
                {
                    Text = "Detail Report",Value = "2",
                },
            };
            return listofReport;
        }

        private List<SelectListItem> ReportsList()
        {
            List<SelectListItem> listofReport = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "---Select---",Value = "",
                },
                new SelectListItem()
                {
                    Text = "User Report",Value = "1",
                }
            };
            return listofReport;
        }
    }
}
