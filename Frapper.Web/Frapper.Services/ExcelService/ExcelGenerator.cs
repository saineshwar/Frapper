using System;
using System.Collections.Generic;
using System.Drawing;
using Frapper.Repository.Reports.Queries;
using Frapper.ViewModel.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frapper.Services.ExcelService
{
    public class ExcelGenerator : IExcelGenerator
    {
        private readonly IExportReportQueries _exportReportQueries;
        public ExcelGenerator(IExportReportQueries exportReportQueries)
        {
            _exportReportQueries = exportReportQueries;
        }
        public ExcelResponse SummaryDownloadReport(string fromdate, string todate)
        {
            string reportname = $"Summary_{Guid.NewGuid():N}.xlsx";
            var list = _exportReportQueries.GetDateWiseUserSummaryReport(fromdate, todate);
            var exportbytes = ExporttoExcel<UserSummaryReport>(list, reportname);

            ExcelResponse excelResponse = new ExcelResponse()
            {
                ExcelBytes = exportbytes,
                ExcelName = reportname
            };
            return excelResponse;
        }

        public ExcelResponse DetailsDownloadReport(string fromdate, string todate)
        {
            string reportname = $"Detail_{Guid.NewGuid():N}.xlsx";
            var list = _exportReportQueries.GetDateWiseUserDetailsReport(fromdate, todate);
            var exportbytes = ExporttoExcel<UserDetailsReport>(list, reportname);
            ExcelResponse excelResponse = new ExcelResponse()
            {
                ExcelBytes = exportbytes,
                ExcelName = reportname
            };
            return excelResponse;
        }

        private byte[] ExporttoExcel<T>(List<T> table, string filename)
        {

            using (ExcelPackage pack = new ExcelPackage())
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
                ws.Cells["A1"].LoadFromCollection(table, true, TableStyles.Dark1);
                return pack.GetAsByteArray();
            }
        }


    }
}