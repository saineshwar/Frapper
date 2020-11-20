using System.Collections.Generic;
using Frapper.ViewModel.Reports;

namespace Frapper.Repository.Reports.Queries
{
    public interface IExportReportQueries
    {
        List<UserSummaryReport> GetDateWiseUserSummaryReport(string fromdate, string todate);
        List<UserDetailsReport> GetDateWiseUserDetailsReport(string fromdate, string todate);
    }
}