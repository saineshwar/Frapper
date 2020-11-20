using Frapper.ViewModel.Reports;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Services.ExcelService
{
    public interface IExcelGenerator
    {
        ExcelResponse SummaryDownloadReport(string fromdate, string todate);
        ExcelResponse DetailsDownloadReport(string fromdate, string todate);
    }
}