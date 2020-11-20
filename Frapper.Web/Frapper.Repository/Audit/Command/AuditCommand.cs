using System;
using System.Data;
using Dapper;
using Frapper.ViewModel.Audit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace Frapper.Repository.Audit.Command
{
    public class AuditCommand : IAuditCommand
    {
        private readonly IConfiguration _configuration;
        public AuditCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void InsertAuditData(AuditTbModel objaudittb)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("AuditDatabaseConnection")))
                {
                    sqlConnection.Open();
                    SqlTransaction dbTransaction = sqlConnection.BeginTransaction();
                    var para = new DynamicParameters();
                    para.Add("@UserID", objaudittb.UserId);
                    para.Add("@SessionID", objaudittb.SessionId);
                    para.Add("@IPAddress", objaudittb.IpAddress);
                    para.Add("@PageAccessed", objaudittb.PageAccessed);
                    para.Add("@LoggedInAt", objaudittb.LoggedInAt);
                    para.Add("@LoggedOutAt", objaudittb.LoggedOutAt);
                    para.Add("@LoginStatus", objaudittb.LoginStatus);
                    para.Add("@ControllerName", objaudittb.ControllerName);
                    para.Add("@ActionName", objaudittb.ActionName);
                    para.Add("@UrlReferrer", objaudittb.UrlReferrer);
                    var result = sqlConnection.Execute("Usp_AuditTB", para, dbTransaction, 0, CommandType.StoredProcedure);

                    if (result>0)
                    {
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}