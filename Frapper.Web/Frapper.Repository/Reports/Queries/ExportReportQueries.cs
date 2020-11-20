using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Frapper.ViewModel.Reports;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Frapper.Repository.Reports.Queries
{
    public class ExportReportQueries : IExportReportQueries
    {
        private readonly IConfiguration _configuration;
        public ExportReportQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<UserSummaryReport> GetDateWiseUserSummaryReport(string fromdate, string todate)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection")))
                {
                    var param = new DynamicParameters();
                    param.Add("@fromdate", fromdate);
                    param.Add("@todate", todate);
                    return con.Query<UserSummaryReport>("Usp_GetDateWiseUserSummary_Report", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserDetailsReport> GetDateWiseUserDetailsReport(string fromdate, string todate)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DatabaseConnection")))
                {
                    var param = new DynamicParameters();
                    param.Add("@fromdate", fromdate);
                    param.Add("@todate", todate);
                    return con.Query<UserDetailsReport>("Usp_GetDateWiseUserDetails_Report", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}